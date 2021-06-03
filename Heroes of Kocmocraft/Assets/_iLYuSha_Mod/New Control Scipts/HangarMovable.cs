using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Kocmoca
{
    public class HangarMovable : MonoBehaviour
    {
        [Header("Database")]
        public KocmocraftDatabase database;
        public Transform[] apron, hangar;
        public Transform apronView, hangarView;

        public float radius;
        private int hangarIndex;


        public int hangarCount;

        [Header("Scene - Hangar Parameter")]
        public Prototype[] prototype;
        public PilotManager[] pilot;
        public CinemachineFreeLook[] cmFreeLook;
        public CinemachineVirtualCamera[] cmCockpit;

        // Start is called before the first frame update
        void Awake()
        {
            // int count = signboards.Length;
            // for (int i = 0; i < count; i++)
            // {
            //     DestroyImmediate(signboards[i]);
            //     DestroyImmediate(stickers[i]);
            // }

            int countApron = apron.Length;
            // signboards = new GameObject[countApron];
            // stickers = new GameObject[countApron];
            hangarCount = hangar.Length;
            prototype = new Prototype[hangarCount];
            pilot = new PilotManager[hangarCount];
            cmFreeLook = new CinemachineFreeLook[hangarCount];
            cmCockpit = new CinemachineVirtualCamera[hangarCount];
            // radioClips = new AudioClip[hangarCount];
            for (int i = 0; i < countApron; i++)
            {
                apron[i].localPosition = new Vector3(630 - (i % 12 / 3) * 360 - i % 3 * 90, 0, 0);
                // signboards[i] = Instantiate(Signboard, apron[i]);
                // signboards[i].GetComponentsInChildren<MeshRenderer>()[0].material = OKB[i];
                // signboards[i].GetComponentsInChildren<MeshRenderer>()[1].material = OKB[i];
                // signboards[i].GetComponentsInChildren<MeshRenderer>()[2].material = OKB[i];

                // stickers[i] = Instantiate(Sticker, apron[i]);
                // stickers[i].GetComponentsInChildren<SpriteRenderer>()[0].sprite = spritesHangarNumber[i];
                // stickers[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = spritesHangarNumber[i];
                // stickers[i].GetComponentsInChildren<SpriteRenderer>()[2].sprite = spritesKocmocraftName[i];

                if (i < hangarCount)
                {
                    hangar[i].localPosition = new Vector3(630 - (i % 12 / 3) * 360 - i % 3 * 90, hangar[i].GetComponentInChildren<BoxCollider>().size.y * 0.5f + 2, 0);
                    prototype[i] = hangar[i].GetComponentInChildren<Prototype>();
                    pilot[i] = hangar[i].GetComponentInChildren<PilotManager>();
                    cmFreeLook[i] = prototype[i].cmFreeLook;
                    cmFreeLook[i].enabled = true;
                    cmFreeLook[i].m_Orbits[0].m_Height = database.kocmocraft[i].design.view.orthoSize + 3;
                    cmFreeLook[i].m_Orbits[2].m_Height = -database.kocmocraft[i].design.view.orthoSize;
                    cmFreeLook[i].m_Orbits[0].m_Radius = database.kocmocraft[i].design.view.near;
                    cmFreeLook[i].m_Orbits[1].m_Radius = 11;
                    cmFreeLook[i].m_Orbits[2].m_Radius = database.kocmocraft[i].design.view.near;
                    cmFreeLook[i].enabled = false;

                    cmCockpit[i] = hangar[i].GetComponent<AvionicsSystem>().cockpitView;
                    // radioClips[i] = database.kocmocraft[i].radio;
                }
            }



        }

        void Start()
        {
            MoveHangarRail();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(Controller.KEY_NextHangar))
            {
                hangarIndex = (int)Mathf.Repeat(++hangarIndex, hangarCount);
                MoveHangarRail();
            }
            else if (Input.GetKeyDown(Controller.KEY_PreviousHangar))
            {
                hangarIndex = (int)Mathf.Repeat(--hangarIndex, hangarCount);
                MoveHangarRail();
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                // panel.localPosition = KocmocaData.invisible; UI
                cmFreeLook[hangarIndex].m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
                cmFreeLook[hangarIndex].m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");
            }
            else
            {
                cmFreeLook[hangarIndex].m_XAxis.m_InputAxisValue = 0;
                cmFreeLook[hangarIndex].m_YAxis.m_InputAxisValue = 0;
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                radius = Mathf.Clamp(radius -= Input.GetAxis("Mouse ScrollWheel") * 37, database.kocmocraft[hangarIndex].design.view.near, 18.2f);
            }
            for (int i = 0; i < 2; i++)
            {
                cmFreeLook[hangarIndex].m_Orbits[i].m_Radius = Mathf.Lerp(cmFreeLook[hangarIndex].m_Orbits[i].m_Radius, radius, 0.073f);
            }
        }

        void MoveHangarRail()
        {
            apronView.SetPositionAndRotation(apron[hangarIndex].position, apron[hangarIndex].rotation);
            hangarView.SetPositionAndRotation(hangar[hangarIndex].position, hangar[hangarIndex].rotation);

            radius = cmFreeLook[hangarIndex].m_Orbits[0].m_Radius;
            for (int i = 0; i < hangarCount; i++)
            {
                cmFreeLook[i].enabled = false;
                cmCockpit[i].enabled = false;
            }
            // if (!isCockpitView)
            cmFreeLook[hangarIndex].enabled = true;
            // else
            //     cmCockpit[hangarIndex].enabled = true;
        }
    }

}
