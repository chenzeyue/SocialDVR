using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;

public class MyNetworkedObject : MonoBehaviour
{
    NetworkContext context;

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
    }

    Vector3 lastPosition;
    Quaternion lastRotation;

    // Update is called once per frame
    void Update()
    {
        if (lastPosition != transform.localPosition || lastRotation != transform.localRotation)
        {
            lastPosition = transform.localPosition;
            lastRotation = transform.localRotation;
            context.SendJson(new Message()
            {
                position = transform.localPosition,
                rotation = transform.localRotation
            });
        }
    }

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();

        // Use the message to update the Component
        transform.localPosition = m.position;
        transform.localRotation = m.rotation;

        // Make sure the logic in Update doesn't trigger as a result of this message
        lastPosition = transform.localPosition;
        lastRotation = transform.localRotation;
    }
}