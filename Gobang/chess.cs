using UnityEngine;
using System.Collections;

public class chess : MonoBehaviour
{

	//�������ϡ����ϡ����¡������ĸ�ê�㣬���ڼ����������
	public GameObject LeftTop;
	public GameObject RightTop;
	public GameObject LeftBottom;
	public GameObject RightBottom;

	public Camera cam;
	//ê������Ļ�ϵ�ӳ��λ��
	Vector3 LTPos;
	Vector3 RTPos;
	Vector3 LBPos;
	Vector3 RBPos;

	Vector3 PointPos;//��ǰ��ѡ��λ��
	float gridWidth = 1; //����������
	float gridHeight = 1; //��������߶�
	float minGridDis;  //�����͸��н�С��һ��
	Vector2[,] chessPos; //�洢���������п������ӵ�λ��
	int[,] chessState; //�洢����λ���ϵ�����״̬
	enum turn { black, white };
	turn chessTurn; //����˳��
	public Texture2D white; //������
	public Texture2D black; //������
	public Texture2D blackWin; //���ӻ�ʤ��ʾͼ
	public Texture2D whiteWin; //���ӻ�ʤ��ʾͼ
	int winner = 0; //��ʤ����1Ϊ���ӣ�-1Ϊ����
	bool isPlaying = true; //�Ƿ��ڶ���״̬
	void Start()
	{
		chessPos = new Vector2[15, 15];
		chessState = new int[15, 15];
		chessTurn = turn.black;

	}

	void Update()
	{
		//����ê��λ��
		LTPos = cam.WorldToScreenPoint(LeftTop.transform.position);
		RTPos = cam.WorldToScreenPoint(RightTop.transform.position);
		LBPos = cam.WorldToScreenPoint(LeftBottom.transform.position);
		RBPos = cam.WorldToScreenPoint(RightBottom.transform.position);
		//����������
		gridWidth = (RTPos.x - LTPos.x) / 14;
		gridHeight = (LTPos.y - LBPos.y) / 14;
		minGridDis = gridWidth < gridHeight ? gridWidth : gridHeight;
		//�������ӵ�λ��
		for (int i = 0; i < 15; i++)
		{
			for (int j = 0; j < 15; j++)
			{
				chessPos[i, j] = new Vector2(LBPos.x + gridWidth * i, LBPos.y + gridHeight * j);
			}
		}
		//���������벢ȷ������״̬
		if (isPlaying && Input.GetMouseButtonDown(0))
		{
			PointPos = Input.mousePosition;
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					//�ҵ���ӽ������λ�õ����ӵ㣬�����������
					if (Dis(PointPos, chessPos[i, j]) < minGridDis / 2 && chessState[i, j] == 0)
					{
						//��������˳��ȷ��������ɫ
						chessState[i, j] = chessTurn == turn.black ? 1 : -1;
						//���ӳɹ�����������˳��
						chessTurn = chessTurn == turn.black ? turn.white : turn.black;
					}
				}
			}
			//�����жϺ�����ȷ���Ƿ��л�ʤ��
			int re = result();
			if (re == 1)
			{
				Debug.Log("����ʤ");
				winner = 1;
				isPlaying = false;
			}
			else if (re == -1)
			{
				Debug.Log("����ʤ");
				winner = -1;
				isPlaying = false;
			}
		}
		//���¿ո����¿�ʼ��Ϸ
		if (Input.GetKeyDown(KeyCode.Space))
		{
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					chessState[i, j] = 0;
				}
			}
			isPlaying = true;
			chessTurn = turn.black;
			winner = 0;
		}
	}
	//����ƽ����뺯��
	float Dis(Vector3 mPos, Vector2 gridPos)
	{
		return Mathf.Sqrt(Mathf.Pow(mPos.x - gridPos.x, 2) + Mathf.Pow(mPos.y - gridPos.y, 2));
	}

	void OnGUI()
	{
		//��������
		for (int i = 0; i < 15; i++)
		{
			for (int j = 0; j < 15; j++)
			{
				if (chessState[i, j] == 1)
				{
					GUI.DrawTexture(new Rect(chessPos[i, j].x - gridWidth / 2, Screen.height - chessPos[i, j].y - gridHeight / 2, gridWidth, gridHeight), black);
				}
				if (chessState[i, j] == -1)
				{
					GUI.DrawTexture(new Rect(chessPos[i, j].x - gridWidth / 2, Screen.height - chessPos[i, j].y - gridHeight / 2, gridWidth, gridHeight), white);
				}
			}
		}
		//���ݻ�ʤ״̬��������Ӧ��ʤ��ͼƬ
		if (winner == 1)
			GUI.DrawTexture(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.25f), blackWin);
		if (winner == -1)
			GUI.DrawTexture(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.25f), whiteWin);

	}
	//ʤ���ж�
	int result()
	{
		int flag = 0;
		//�����ǰ�ֵ��������ӣ��жϺ����Ƿ��ʤ
		if (chessTurn == turn.white)
		{
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					if (j < 4)
					{
						//����
						if (chessState[i, j] == 1 && chessState[i, j + 1] == 1 && chessState[i, j + 2] == 1 && chessState[i, j + 3] == 1 && chessState[i, j + 4] == 1)
						{
							flag = 1;
							return flag;
						}
						//����
						if (chessState[i, j] == 1 && chessState[i + 1, j] == 1 && chessState[i + 2, j] == 1 && chessState[i + 3, j] == 1 && chessState[i + 4, j] == 1)
						{
							flag = 1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == 1 && chessState[i + 1, j + 1] == 1 && chessState[i + 2, j + 2] == 1 && chessState[i + 3, j + 3] == 1 && chessState[i + 4, j + 4] == 1)
						{
							flag = 1;
							return flag;
						}
					}
					else if (j >= 4 && j < 11)
					{
						//����
						if (chessState[i, j] == 1 && chessState[i, j + 1] == 1 && chessState[i, j + 2] == 1 && chessState[i, j + 3] == 1 && chessState[i, j + 4] == 1)
						{
							flag = 1;
							return flag;
						}
						//����
						if (chessState[i, j] == 1 && chessState[i + 1, j] == 1 && chessState[i + 2, j] == 1 && chessState[i + 3, j] == 1 && chessState[i + 4, j] == 1)
						{
							flag = 1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == 1 && chessState[i + 1, j + 1] == 1 && chessState[i + 2, j + 2] == 1 && chessState[i + 3, j + 3] == 1 && chessState[i + 4, j + 4] == 1)
						{
							flag = 1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == 1 && chessState[i + 1, j - 1] == 1 && chessState[i + 2, j - 2] == 1 && chessState[i + 3, j - 3] == 1 && chessState[i + 4, j - 4] == 1)
						{
							flag = 1;
							return flag;
						}
					}
					else
					{
						//����
						if (chessState[i, j] == 1 && chessState[i + 1, j] == 1 && chessState[i + 2, j] == 1 && chessState[i + 3, j] == 1 && chessState[i + 4, j] == 1)
						{
							flag = 1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == 1 && chessState[i + 1, j - 1] == 1 && chessState[i + 2, j - 2] == 1 && chessState[i + 3, j - 3] == 1 && chessState[i + 4, j - 4] == 1)
						{
							flag = 1;
							return flag;
						}
					}

				}
			}
			for (int i = 11; i < 15; i++)
			{
				for (int j = 0; j < 11; j++)
				{
					//ֻ��Ҫ�жϺ���  
					if (chessState[i, j] == 1 && chessState[i, j + 1] == 1 && chessState[i, j + 2] == 1 && chessState[i, j + 3] == 1 && chessState[i, j + 4] == 1)
					{
						flag = 1;
						return flag;
					}
				}
			}
		}
		//�����ǰ�ֵ��������ӣ��жϰ����Ƿ��ʤ
		else if (chessTurn == turn.black)
		{
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					if (j < 4)
					{
						//����
						if (chessState[i, j] == -1 && chessState[i, j + 1] == -1 && chessState[i, j + 2] == -1 && chessState[i, j + 3] == -1 && chessState[i, j + 4] == -1)
						{
							flag = -1;
							return flag;
						}
						//����
						if (chessState[i, j] == -1 && chessState[i + 1, j] == -1 && chessState[i + 2, j] == -1 && chessState[i + 3, j] == -1 && chessState[i + 4, j] == -1)
						{
							flag = -1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == -1 && chessState[i + 1, j + 1] == -1 && chessState[i + 2, j + 2] == -1 && chessState[i + 3, j + 3] == -1 && chessState[i + 4, j + 4] == -1)
						{
							flag = -1;
							return flag;
						}
					}
					else if (j >= 4 && j < 11)
					{
						//����
						if (chessState[i, j] == -1 && chessState[i, j + 1] == -1 && chessState[i, j + 2] == -1 && chessState[i, j + 3] == -1 && chessState[i, j + 4] == -1)
						{
							flag = -1;
							return flag;
						}
						//����
						if (chessState[i, j] == -1 && chessState[i + 1, j] == -1 && chessState[i + 2, j] == -1 && chessState[i + 3, j] == -1 && chessState[i + 4, j] == -1)
						{
							flag = -1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == -1 && chessState[i + 1, j + 1] == -1 && chessState[i + 2, j + 2] == -1 && chessState[i + 3, j + 3] == -1 && chessState[i + 4, j + 4] == -1)
						{
							flag = -1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == -1 && chessState[i + 1, j - 1] == -1 && chessState[i + 2, j - 2] == -1 && chessState[i + 3, j - 3] == -1 && chessState[i + 4, j - 4] == -1)
						{
							flag = -1;
							return flag;
						}
					}
					else
					{
						//����
						if (chessState[i, j] == -1 && chessState[i + 1, j] == -1 && chessState[i + 2, j] == -1 && chessState[i + 3, j] == -1 && chessState[i + 4, j] == -1)
						{
							flag = -1;
							return flag;
						}
						//��б��
						if (chessState[i, j] == -1 && chessState[i + 1, j - 1] == -1 && chessState[i + 2, j - 2] == -1 && chessState[i + 3, j - 3] == -1 && chessState[i + 4, j - 4] == -1)
						{
							flag = -1;
							return flag;
						}
					}
				}
			}
			for (int i = 11; i < 15; i++)
			{
				for (int j = 0; j < 11; j++)
				{
					//ֻ��Ҫ�жϺ���  
					if (chessState[i, j] == -1 && chessState[i, j + 1] == -1 && chessState[i, j + 2] == -1 && chessState[i, j + 3] == -1 && chessState[i, j + 4] == -1)
					{
						flag = -1;
						return flag;
					}
				}
			}
		}
		return flag;
	}
}
