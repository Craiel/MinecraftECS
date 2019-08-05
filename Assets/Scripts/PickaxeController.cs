using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;

namespace Minecraft
{
    public class PickaxeController : MonoBehaviour
    {
        //Vector2 mouseLook;

        public LayerMask blockLayer;
        //private static Minecraft.GameSettings MatRef;

        GameObject player;
        //public GameObject playerEntity;
        public static Transform BlockToDestroy;

        BlockType BlockToPlace;

        public static int blockID = 1;


        public AudioClip grass_audio;
        public AudioClip stone_audio;
        public AudioClip dirt_audio;
        public AudioClip wood_audio;

        AudioSource AS;

        public ParticleSystem digEffect;

        //bool stepAudioIsPlaying = false;

        void Start()
        {
            AS = transform.GetComponent<AudioSource>();
            player = this.transform.parent.gameObject;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            /*
            if (Input.GetButtonUp("Q") && Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetButtonDown("Q") && Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }*/

            if (blockID > 7)
            {
                blockID = 1;
            }
            if (blockID < 1)
            {
                blockID = 7;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                blockID++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                blockID--;
            }

            #region // blocklist
            if (blockID == 1)
            {
                //stone
                BlockToPlace = BlockType.Stone;
            }
            else if (blockID == 2)
            {
                //plank
                BlockToPlace = BlockType.Plank;
            }
            else if (blockID == 3)
            {
                //glass
                BlockToPlace = BlockType.Glass;
            }
            else if (blockID == 4)
            {
                //wood
                BlockToPlace = BlockType.Wood;
            }
            else if (blockID == 5)
            {
                //cobble
                BlockToPlace = BlockType.CobbleStone;
            }
            else if (blockID == 6)
            {
                //TNT
                BlockToPlace = BlockType.TNT;
            }
            else if (blockID == 7)
            {
                //brick
                BlockToPlace = BlockType.Brick;
            }
            #endregion

            //right click to place block
            if (Input.GetMouseButtonDown(1))
            {
                PlaceBlock(BlockToPlace);
            }

            //left click to dig
            if (Input.GetMouseButtonDown(0))
            {
                DestroyBlock();
            }

        }

        /*
        IEnumerator PlayStep(AudioClip audioClip)
        {
            stepAudioIsPlaying = true;
            AS.PlayOneShot(audioClip, 0.1f);
            yield return new WaitForSecondsRealtime(0.5f);
            stepAudioIsPlaying = false;
        }*/

        void PlaceBlock(BlockType type)
        {
            Physics.Raycast(transform.position, transform.forward, out var hitInfo, 7, blockLayer);
            if (hitInfo.transform != null)
            {
                if (blockID == 1 || blockID == 3 || blockID == 5 || blockID == 7)
                {
                    AS.PlayOneShot(stone_audio);
                }
                else if (blockID == 2 || blockID == 4)
                {
                    AS.PlayOneShot(wood_audio);
                }

                Vector3 blockPosition = hitInfo.transform.position + hitInfo.normal;
                var data = new BlockCreateData
                {
                    X = (int)blockPosition.x,
                    Y = (int)blockPosition.y,
                    Z = (int)blockPosition.z,
                    BlockType = type
                };

                Entity entity = World.Active.EntityManager.CreateEntity(GameSettingsData.Instance.BlockCreateArchetype);
                World.Active.EntityManager.SetComponentData(entity, data);
            }
        }

        void DestroyBlock()
        {
            Physics.Raycast(transform.position, transform.forward, out var hitInfo, 7, blockLayer);
            if (hitInfo.transform != null)
            {
                Vector3 blockPosition = hitInfo.transform.position + hitInfo.normal;
                //move the dig effect to the position and play
                if (digEffect && !digEffect.isPlaying)
                {
                    digEffect.transform.position = blockPosition;
                    digEffect.Play();
                }

                AS.PlayOneShot(dirt_audio);

                var data = new BlockDestroyData
                {
                    X = (int) blockPosition.x,
                    Y = (int) blockPosition.y,
                    Z = (int) blockPosition.z
                };

                Entity entity = World.Active.EntityManager.CreateEntity(GameSettingsData.Instance.BlockDestroyArchetype);
                World.Active.EntityManager.SetComponentData(entity, data);
            }
        }
    }
}