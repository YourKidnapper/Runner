﻿using System;
using UnityEngine;

namespace RayFire
{
    [DisallowMultipleComponent]
    [AddComponentMenu ("RayFire/Rayfire Man")]
    [HelpURL ("https://rayfirestudios.com/unity-online-help/components/unity-man-component/")]
    public class RayfireMan : MonoBehaviour
    {
        // UI
        public bool                   setGravity;
        public float                  multiplier       = 1f;
        public RigidbodyInterpolation interpolation    = RigidbodyInterpolation.None;
        public float                  colliderSize     = 0.05f;
        public CollisionDetectionMode meshCollision    = CollisionDetectionMode.ContinuousDynamic;
        public CollisionDetectionMode clusterCollision = CollisionDetectionMode.Discrete;
        public float                  minimumMass      = 0.1f;
        public float                  maximumMass      = 400f;
        public RFMaterialPresets      materialPresets  = new RFMaterialPresets();
        public GameObject             parent;
        public float                  globalSolidity               = 1f;
        public float                  timeQuota                    = 0.033f;
        public RFManDemolition        advancedDemolitionProperties = new RFManDemolition();
        public RFPoolingFragment      fragments                    = new RFPoolingFragment();
        public RFPoolingParticles     particles                    = new RFPoolingParticles();
        public RFStorage              storage;
        
        // Non Serialized
        [NonSerialized] public Transform transForm;
        [NonSerialized] public float     maxTimeThisFrame;

        // Static
        public static RayfireMan inst;
        public static int        buildMajor         = 1;
        public static int        buildMinor         = 56;
        public static float      colliderSizeStatic = 0.05f;
        
        /// /////////////////////////////////////////////////////////
        /// Common
        /// /////////////////////////////////////////////////////////
        
        // Awake
        void Awake()
        {
            // Set static instance
            SetInstance();
            
            //System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
        }

        // Late Update
        void LateUpdate()
        {
            maxTimeThisFrame = 0f;
        }

        void Reset()
        {
            SetInstance();
        }
        
        /// /////////////////////////////////////////////////////////
        /// Instance
        /// /////////////////////////////////////////////////////////

        // Set instance
        void SetInstance()
        {
            // Inst not define, set to this
            if (inst == null)
            {
                inst = this;
            }

            // Inst defined
            if (inst != null)
            {
                // Instance is this mono - > Init
                if (inst == this)
                {
                    // Set vars
                    SetVariables();

                    // Runtime ops
                    if (Application.isPlaying == true) 
                    {
                        // Start pooling objects for fragments
                        SetPooling();

                        // Create storage and stat root check coroutine
                        SetStorage();
                    }
                }

                // Inst is not this mono. Destroy.
                if (inst != this)
                {
                    if (Application.isPlaying == true)
                        Destroy (gameObject);
                    else if (Application.isEditor == true)
                        DestroyImmediate (gameObject);
                }
            }
        }

        /// /////////////////////////////////////////////////////////
        /// Enable/Disable
        /// /////////////////////////////////////////////////////////
        
        // Disable
        void OnDisable()
        {
            fragments.inProgress = false;
            particles.inProgress = false;
            storage.inProgress   = false;
        }

        // Activation
        void OnEnable()
        {
            if (Application.isPlaying == true && gameObject.activeSelf == true)
            {
                // Continue pooling
                SetPooling();
                
                // Continue storage check
                SetStorage();
            }
        }

        /// /////////////////////////////////////////////////////////
        /// Methods
        /// /////////////////////////////////////////////////////////

        // Set vars
        void SetVariables()
        {
            // Get components
            transForm = GetComponent<Transform>();

            // Reset amount
            advancedDemolitionProperties.ResetCurrentAmount();

            // Set gravity
            SetGravity();

            // Set Physic Materials if needed
            materialPresets.SetMaterials();

            colliderSizeStatic = colliderSize;
        }

        // Set gravity
        void SetGravity()
        {
            if (setGravity == true)
                Physics.gravity = -9.81f * multiplier * Vector3.up;
        }

        /// /////////////////////////////////////////////////////////
        /// Other
        /// /////////////////////////////////////////////////////////
        
        // Create RayFire manager if not created
        public static void RayFireManInit()
        {
            if (inst == null)
            {
                GameObject rfMan = new GameObject ("RayFireMan");
                inst = rfMan.AddComponent<RayfireMan>();
            }

            if (Application.isPlaying == false)
            {
                inst.SetInstance();
            }
        }
        
        // Max fragments amount check
        public static bool MaxAmountCheck
        {
            get
            {
                if (inst.advancedDemolitionProperties.currentAmount < inst.advancedDemolitionProperties.maximumAmount)
                    return true;

                inst.advancedDemolitionProperties.AmountWarning();
                return false;
            }
        }
        
        /// /////////////////////////////////////////////////////////
        /// Pooling
        /// /////////////////////////////////////////////////////////

        // Enable objects pooling for fragments                
        void SetPooling()
        {
            // Create pool root
            fragments.CreatePoolRoot (transform);

            // Create pool instance
            fragments.CreateInstance (transform);

            // Pooling. Mot in editor
            if (Application.isPlaying == true && fragments.enable == true && fragments.inProgress == false)
                StartCoroutine (fragments.StartPoolingCor (transForm));

            // Create pool root
            particles.CreatePoolRoot (transform);

            // Create pool instance
            particles.CreateInstance ();

            // Pooling. Mot in editor
            if (Application.isPlaying == true && particles.enable == true && particles.inProgress == false)
                StartCoroutine (particles.StartPoolingCor ());
        }
        
        /// /////////////////////////////////////////////////////////
        /// Storage
        /// /////////////////////////////////////////////////////////
        
        // Create storage root
        void SetStorage()
        {
            // Create
            if (storage == null)
                storage = new RFStorage();
            
            // Create storage if has no
            storage.CreateStorageRoot (transform);
            
            // Start empty root removing coroutine if not running
            if (Application.isPlaying == true && storage.inProgress == false)
                StartCoroutine (storage.StorageCor ());
        }

        // Destroy all storage objects
        public void DestroyStorage()
        {
            storage.DestroyAll();
        }

        /// /////////////////////////////////////////////////////////
        /// Parent
        /// /////////////////////////////////////////////////////////

        // Set root to manager or to the same parent
        public static void SetParentByManager (Transform tm, Transform original, bool noRegister = false)
        {
            if (inst != null && inst.advancedDemolitionProperties.parent == RFManDemolition.FragmentParentType.Manager)
            {
                tm.parent = inst.storage.storageRoot;
            }
            else if (original == null || original.parent == null)
            {
                tm.parent = inst.storage.storageRoot;
            }
            else
                tm.parent = original.parent;

            // Register in storage
            if (noRegister == false)
                inst.storage.Register (tm);
        }
        
        // Set root to manager or to the same parent
        public static void SetParentByManager (Transform tm)
        {
            if (inst != null && inst.advancedDemolitionProperties.parent == RFManDemolition.FragmentParentType.Manager)
                tm.parent = inst.storage.storageRoot;
            
            // Register in storage
            inst.storage.Register (tm);
        }

        /// /////////////////////////////////////////////////////////
        /// Destroy/Deactivate Fragment/Shard
        /// /////////////////////////////////////////////////////////

        // Check if fragment is the last child in root and delete root as well
        public static void DestroyFragment (RayfireRigid scr, Transform tm, float time = 0f)
        {
            // Decrement total amount.
            if (Application.isPlaying == true)
                inst.advancedDemolitionProperties.currentAmount--;
            
            // Deactivate
            scr.gameObject.SetActive (false);

            // Destroy
            if (scr.reset.action == RFReset.PostDemolitionType.DestroyWithDelay)
                DestroyOp (scr, tm, time);
        }
        
        // Destroy rigidroot shard
        public static void DestroyShard (RayfireRigidRoot scr, RFShard shard)
        {
            // Deactivate
            shard.tm.gameObject.SetActive (false);
            
            // Destroy
            if (scr.reset.action == RFReset.PostDemolitionType.DestroyWithDelay)
                DestroyGo (shard.tm.gameObject);
        }
        
        /// /////////////////////////////////////////////////////////
        /// Destroy
        /// /////////////////////////////////////////////////////////
        
        // Check if fragment is the last child in root and delete root as well
        public static void DestroyGo (GameObject go)
        {
            Destroy (go);
        }

        // Check if fragment is the last child in root and delete root as well
        static void DestroyOp (RayfireRigid scr, Transform tm, float time = 0f)
        {
            // Set delay
            if (time == 0)
                time = scr.reset.destroyDelay;

            // Object is going to be destroyed. Timer is on
            scr.reset.toBeDestroyed = true;

            // Destroy object
            inst.fragments.DestroyOrReset (scr, time);;

            // Destroy root
            if (tm != null && tm.childCount == 0)
            {
                // TODO collect root in special roots list, check every 10 seconds and destroy if they are empty
                Destroy (tm.gameObject, time);
            }
        }
    }
}

