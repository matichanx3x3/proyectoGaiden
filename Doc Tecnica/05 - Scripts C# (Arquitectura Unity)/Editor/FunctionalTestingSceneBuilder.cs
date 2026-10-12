#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using System.IO;
using Game.Core;
using Game.Managers;
using Game.Exploration;
using Game.Cameras;
using Game.UI;
using Game.Items;
using Game.Weapons;
using Game.Units;
using Game.Combat;

namespace Game.EditorTools {
    public static class FunctionalTestingSceneBuilder {
        [MenuItem("Game/Build Functional Testing Scene")]
        public static void BuildScene() {
            // 1. Crear nueva escena vacía
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            string scenePath = "Assets/Scenes/01_Functional_Testing.unity";

            // Materiales de prototipado rápidos
            Material matFloor = CreateMaterial("Mat_Test_Floor", new Color(0.2f, 0.22f, 0.25f));
            Material matWall = CreateMaterial("Mat_Test_Wall", new Color(0.12f, 0.14f, 0.16f));
            Material matPlatform = CreateMaterial("Mat_Test_Platform", new Color(0.28f, 0.32f, 0.38f));
            Material matCrate1 = CreateMaterial("Mat_Test_Crate1", new Color(0.75f, 0.45f, 0.15f));
            Material matCrate2 = CreateMaterial("Mat_Test_Crate2", new Color(0.65f, 0.35f, 0.1f));
            Material matDoor = CreateMaterial("Mat_Test_Door", new Color(0.5f, 0.5f, 0.55f));
            Material matKeyDoor = CreateMaterial("Mat_Test_Door_Red", new Color(0.85f, 0.15f, 0.15f));
            Material matLadder = CreateMaterial("Mat_Test_Ladder", new Color(0.9f, 0.75f, 0.2f));
            Material matSocketBlue = CreateMaterial("Mat_Socket_Blue", new Color(0.15f, 0.5f, 0.95f));
            Material matSocketRed = CreateMaterial("Mat_Socket_Red", new Color(0.95f, 0.15f, 0.15f));
            Material matDarkVoid = CreateMaterial("Mat_DarkVoid", new Color(0.02f, 0.02f, 0.02f));
            Material matTransitionDoor = CreateMaterial("Mat_Door_Transition", new Color(0.35f, 0.4f, 0.45f));

            // --- 2. ILUMINACIÓN Y ENTORNO ---
            var lightGo = new GameObject("Directional Light");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.0f;
            light.color = new Color(0.9f, 0.95f, 1.0f);
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // Luz ambiental tenue
            RenderSettings.ambientLight = new Color(0.2f, 0.22f, 0.26f);

            // --- 3. GEOMETRÍA DEL ESCENARIO ---
            var envRoot = new GameObject("[=== ENVIRONMENT ===]");

            // Suelo Principal (Y = 0)
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Floor_Ground";
            floor.transform.SetParent(envRoot.transform);
            floor.transform.position = new Vector3(0f, -0.5f, 0f);
            floor.transform.localScale = new Vector3(26f, 1f, 26f);
            floor.GetComponent<Renderer>().material = matFloor;

            // Paredes Perimetrales
            CreateWall(envRoot.transform, "Wall_North", new Vector3(0f, 2f, 13f), new Vector3(26f, 4f, 1f), matWall);
            CreateWall(envRoot.transform, "Wall_South", new Vector3(0f, 2f, -13f), new Vector3(26f, 4f, 1f), matWall);
            CreateWall(envRoot.transform, "Wall_East", new Vector3(13f, 2f, 0f), new Vector3(1f, 4f, 26f), matWall);
            CreateWall(envRoot.transform, "Wall_West", new Vector3(-13f, 2f, 0f), new Vector3(1f, 4f, 26f), matWall);

            // Pared Divisoria Central con huecos para puertas (Z = 0)
            // Hueco Puerta 1 en X = -3, Hueco Puerta 2 en X = 3
            CreateWall(envRoot.transform, "Wall_Mid_Left", new Vector3(-8f, 2f, 0f), new Vector3(9f, 4f, 0.6f), matWall);
            CreateWall(envRoot.transform, "Wall_Mid_Center", new Vector3(0f, 2f, 0f), new Vector3(4f, 4f, 0.6f), matWall);
            CreateWall(envRoot.transform, "Wall_Mid_Right", new Vector3(8f, 2f, 0f), new Vector3(9f, 4f, 0.6f), matWall);

            // Dinteles superiores sobre las puertas
            CreateWall(envRoot.transform, "Lintel_Door1", new Vector3(-3f, 3.25f, 0f), new Vector3(2f, 1.5f, 0.6f), matWall);
            CreateWall(envRoot.transform, "Lintel_Door2", new Vector3(3f, 3.25f, 0f), new Vector3(2f, 1.5f, 0.6f), matWall);

            // --- 4. VERTICALIDAD: MEZZANINE (SEGUNDO NIVEL Y = 3), ESCALERAS, RAMPA Y ESCALA VERTICAL ---
            var verticalRoot = new GameObject("[=== VERTICALITY & PLATFORMS ===]");

            // Plataforma Mezzanine (Y = 3)
            var mezzanine = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mezzanine.name = "Platform_Mezzanine_Y3";
            mezzanine.transform.SetParent(verticalRoot.transform);
            mezzanine.transform.position = new Vector3(0f, 2.75f, 8f);
            mezzanine.transform.localScale = new Vector3(16f, 0.5f, 8f);
            mezzanine.GetComponent<Renderer>().material = matPlatform;

            // Barandillas del mezzanine con aperturas en X = -3 (escaleras), X = 3 (rampa) y Z = 12 (puerta a nueva zona)
            CreateWall(verticalRoot.transform, "Railing_North_Left", new Vector3(-4.5f, 3.5f, 12f), new Vector3(7f, 1f, 0.2f), matWall);
            CreateWall(verticalRoot.transform, "Railing_North_Right", new Vector3(4.5f, 3.5f, 12f), new Vector3(7f, 1f, 0.2f), matWall);
            CreateWall(verticalRoot.transform, "Railing_West", new Vector3(-8f, 3.5f, 8f), new Vector3(0.2f, 1f, 8f), matWall);
            CreateWall(verticalRoot.transform, "Railing_East", new Vector3(8f, 3.5f, 8f), new Vector3(0.2f, 1f, 8f), matWall);
            CreateWall(verticalRoot.transform, "Railing_South_Left", new Vector3(-6.25f, 3.5f, 4f), new Vector3(3.5f, 1f, 0.2f), matWall);
            CreateWall(verticalRoot.transform, "Railing_South_Center", new Vector3(0f, 3.5f, 4f), new Vector3(3.0f, 1f, 0.2f), matWall);
            CreateWall(verticalRoot.transform, "Railing_South_Right", new Vector3(6.25f, 3.5f, 4f), new Vector3(3.5f, 1f, 0.2f), matWall);

            // Escaleras hacia Mezzanine (Alineadas con Puerta 1 en X = -3.0, suben desde Z = 1.0 hasta Z = 4.0, Y = 0 a Y = 3)
            int stepCount = 10;
            float totalHeight = 3.0f;
            float stepHeight = totalHeight / stepCount; // 0.3m
            float stepDepth = 0.30f;
            float stairsStartX = -3.0f;
            float stairsStartZ = 1.0f;

            for (int i = 0; i < stepCount; i++) {
                var step = GameObject.CreatePrimitive(PrimitiveType.Cube);
                step.name = $"Stair_Step_{i + 1}";
                step.transform.SetParent(verticalRoot.transform);
                float y = (i * stepHeight) + (stepHeight * 0.5f);
                float z = stairsStartZ + (i * stepDepth) + (stepDepth * 0.5f);
                step.transform.position = new Vector3(stairsStartX, y, z);
                step.transform.localScale = new Vector3(2.0f, stepHeight, stepDepth);
                step.GetComponent<Renderer>().material = matPlatform;
            }

            // Rampa alternativa (Alineada con Puerta 2 en X = 3.0, conecta Y=0 en Z=1.0 hasta Y=3 en Z=4.0)
            var ramp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ramp.name = "Ramp_Access";
            ramp.transform.SetParent(verticalRoot.transform);
            ramp.transform.position = new Vector3(3.0f, 1.5f, 2.5f);
            ramp.transform.rotation = Quaternion.Euler(-45f, 0f, 0f); // Pendiente regular 45 grados (slopeLimit = 50f)
            ramp.transform.localScale = new Vector3(2.0f, 0.2f, 4.24f);
            ramp.GetComponent<Renderer>().material = matPlatform;

            // Escala vertical tipo marinero (Ladder_Vertical Prefab) en X = 7.5, sube de Y=0 a Y=3.5
            var ladderPrefab = GetOrCreateLadderPrefab("Ladder_Vertical", matLadder, 3.5f);
            var ladderInst = PrefabUtility.InstantiatePrefab(ladderPrefab, verticalRoot.transform) as GameObject;
            ladderInst.transform.position = new Vector3(7.5f, 0f, 3.8f);

            // Plataforma elevada de pruebas para puzle con caja (Y = 1.2, accesible trepando por la caja)
            CreateWall(verticalRoot.transform, "Puzzle_Elevated_Platform", new Vector3(5f, 0.6f, -1.0f), new Vector3(3.0f, 1.2f, 2.5f), matPlatform);

            // --- 5. PUERTAS INTERACTIVAS (Prefabs Door_Standard, Door_Locked_Red y Door_LevelTransition) ---
            var doorsRoot = new GameObject("[=== INTERACTIVE DOORS ===]");

            // Puerta 1: Puerta estándar desbloqueada (Hueco X = -3, Z = 0)
            var doorStdPrefab = GetOrCreateDoorPrefab("Door_Standard", matDoor, false, ItemId.None, new Vector3(0f, 90f, 0f));
            var door1 = PrefabUtility.InstantiatePrefab(doorStdPrefab, doorsRoot.transform) as GameObject;
            door1.name = "Door_Standard_Unlocked";
            door1.transform.position = new Vector3(-3.8f, 0f, 0f);

            // Puerta 2: Puerta roja con cerrojo de seguridad (Requiere RedKeyCard, Hueco X = 3, Z = 0)
            var doorRedPrefab = GetOrCreateDoorPrefab("Door_Locked_Red", matKeyDoor, true, ItemId.RedKeyCard, new Vector3(0f, -90f, 0f));
            var door2 = PrefabUtility.InstantiatePrefab(doorRedPrefab, doorsRoot.transform) as GameObject;
            door2.name = "Door_Locked_Red";
            door2.transform.position = new Vector3(3.8f, 0f, 0f);

            // Puerta 3: Puerta de Transición a otra zona oscura (Mezzanine Norte, Y = 3.0, Z = 12.0)
            var doorTransPrefab = GetOrCreateLevelTransitionDoorPrefab("Door_LevelTransition", matTransitionDoor, matDarkVoid, new Vector3(0f, 1f, -8f));
            var door3 = PrefabUtility.InstantiatePrefab(doorTransPrefab, doorsRoot.transform) as GameObject;
            door3.name = "Door_LevelTransition_North";
            door3.transform.position = new Vector3(0f, 3.0f, 12.0f);

            // --- 6. PUNTOS DESTINO Y CAJAS EMPUJABLES (Sockets Azules y Rojos) ---
            var socketsRoot = new GameObject("[=== DESTINATION SOCKETS (PUZZLE) ===]");

            // Sockets para Caja 2 (Puzle de plataforma elevada: mover de -4 a -3.2 [Azul] y a -2.1 [Rojo])
            CreateDestinationSocket(socketsRoot.transform, "Socket_Blue_Crate2", BoxDestinationType.Blue, new Vector3(5f, 0.02f, -3.2f), matSocketBlue);
            CreateDestinationSocket(socketsRoot.transform, "Socket_Red_Crate2", BoxDestinationType.Red, new Vector3(5f, 0.02f, -2.1f), matSocketRed);

            // Sockets para Caja 1 (Pasillo Oeste: mover de -4 a -2.5 [Azul] y a -1.0 [Rojo])
            CreateDestinationSocket(socketsRoot.transform, "Socket_Blue_Crate1", BoxDestinationType.Blue, new Vector3(-3f, 0.02f, -2.5f), matSocketBlue);
            CreateDestinationSocket(socketsRoot.transform, "Socket_Red_Crate1", BoxDestinationType.Red, new Vector3(-3f, 0.02f, -1.0f), matSocketRed);

            var cratesRoot = new GameObject("[=== PUSHABLE BOXES ===]");
            var cratePrefab = GetOrCreatePushableBoxPrefab("PushableBox_Puzzle", matCrate1, 25f, 2.4f, 0f);

            // Caja 1 (En el pasillo sur)
            var crate1 = PrefabUtility.InstantiatePrefab(cratePrefab, cratesRoot.transform) as GameObject;
            crate1.name = "PushableCrate_1";
            crate1.transform.position = new Vector3(-3f, 0.4f, -4f);

            // Caja 2 (Frente a la plataforma elevada: al llevarla al socket rojo se bloquea y despliega el escalón)
            var crate2 = PrefabUtility.InstantiatePrefab(cratePrefab, cratesRoot.transform) as GameObject;
            crate2.name = "PushableCrate_2";
            crate2.transform.position = new Vector3(5f, 0.4f, -4f);

            // --- 7. OBJETOS EN EL SUELO (Pickups Prefabs) ---
            var pickupsRoot = new GameObject("[=== PICKUPS ===]");

            // Green Herb en el área sur
            var herbPrefab = GetOrCreatePickupPrefab("Pickup_GreenHerb", ItemId.GreenHerb, Color.green);
            var herbInst = PrefabUtility.InstantiatePrefab(herbPrefab, pickupsRoot.transform) as GameObject;
            herbInst.transform.position = new Vector3(-6f, 0.3f, -4f);

            // First Aid Med en la plataforma de puzle (recompensa por trepar)
            var medPrefab = GetOrCreatePickupPrefab("Pickup_FirstAidMed", ItemId.FirstAidMed, Color.cyan);
            var medInst = PrefabUtility.InstantiatePrefab(medPrefab, pickupsRoot.transform) as GameObject;
            medInst.transform.position = new Vector3(5f, 1.45f, -1.0f);

            // LLAVE ROJA (RedKeyCard) en el Mezzanine (Y = 3.3)
            // Obliga al jugador a usar las escaleras / verticalidad para conseguir la llave que abre la puerta roja!
            var redKeyPrefab = GetOrCreatePickupPrefab("Pickup_Key_Red", ItemId.RedKeyCard, new Color(1f, 0.15f, 0.15f));
            var keyInst = PrefabUtility.InstantiatePrefab(redKeyPrefab, pickupsRoot.transform) as GameObject;
            keyInst.transform.position = new Vector3(0f, 3.3f, 9.5f);

            // --- 8. ENEMIGO DE PRUEBAS PARA COMBATE ADITIVO (En cámara lateral) ---
            var enemyGo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemyGo.name = "Enemy_Overworld_Test";
            enemyGo.transform.position = new Vector3(-9f, 1f, 8f);
            enemyGo.GetComponent<Renderer>().material = CreateMaterial("Mat_Enemy_Test", Color.red);
            var enemyCol = enemyGo.GetComponent<CapsuleCollider>();
            enemyCol.isTrigger = true;
            enemyGo.AddComponent<EnemyOverworldUnit>();

            // --- 9. JUGADOR (Player_HeroShell) ---
            var playerGo = new GameObject("Player_HeroShell");
            playerGo.tag = "Player";
            playerGo.layer = 0;
            playerGo.transform.position = new Vector3(0f, 1f, -8f);

            var cc = playerGo.AddComponent<CharacterController>();
            cc.radius = 0.35f;
            cc.height = 1.8f;
            cc.center = Vector3.zero;
            cc.stepOffset = 0.45f;
            cc.slopeLimit = 50f;

            var explCtrl = playerGo.AddComponent<ExplorationController>();

            // Modelo visual del jugador (Cápsula)
            var pVisual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            pVisual.name = "HeroModel_Shell";
            pVisual.transform.SetParent(playerGo.transform, false);
            pVisual.transform.localPosition = Vector3.zero;
            pVisual.GetComponent<Renderer>().material = CreateMaterial("Mat_Player_Test", new Color(0.2f, 0.6f, 0.9f));
            Object.DestroyImmediate(pVisual.GetComponent<Collider>()); // Evitar colisión duplicada con CC

            // Visor / indicador frontal para ver orientación
            var pVisor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pVisor.name = "Visor_Heading";
            pVisor.transform.SetParent(pVisual.transform, false);
            pVisor.transform.localPosition = new Vector3(0f, 0.4f, 0.3f);
            pVisor.transform.localScale = new Vector3(0.4f, 0.15f, 0.25f);
            pVisor.GetComponent<Renderer>().material = CreateMaterial("Mat_Visor", Color.white);
            Object.DestroyImmediate(pVisor.GetComponent<Collider>());

            // --- 10. CÁMARA (Main Camera + CameraController) ---
            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Skybox;
            cam.fieldOfView = 50f;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 100f;
            camGo.AddComponent<AudioListener>();

            var camCtrl = camGo.AddComponent<CameraController>();
            camCtrl.SetTarget(playerGo.transform);

            // Posición inicial de la cámara
            camGo.transform.position = playerGo.transform.position + new Vector3(0f, 8.5f, -6.5f);
            camGo.transform.rotation = Quaternion.Euler(45f, 0f, 0f);

            // --- 11. GESTORES DE ESCENA [=== SCENE MANAGERS ===] ---
            var mgrRoot = new GameObject("[=== SCENE MANAGERS ===]");
            mgrRoot.AddComponent<GameManager>();
            mgrRoot.AddComponent<UnitManager>();
            mgrRoot.AddComponent<PartyManager>();
            mgrRoot.AddComponent<CombatManager>();
            mgrRoot.AddComponent<InventorySystem>();

            // --- 12. INTERFACES DE USUARIO (HUD, INVENTARIO, GAMEOVER) ---
            var uiRoot = new GameObject("[=== UI CANVASES ===]");

            // EventSystem con Input System Module
            var eventGo = new GameObject("EventSystem");
            eventGo.transform.SetParent(uiRoot.transform);
            eventGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventGo.AddComponent<InputSystemUIInputModule>();

            // HUD Canvas
            var hudGo = new GameObject("HUD_Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            hudGo.transform.SetParent(uiRoot.transform);
            var hudCanvas = hudGo.GetComponent<Canvas>();
            hudCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var hudComp = hudGo.AddComponent<ExplorationHUD>();
            hudComp.EnsureUIHierarchy();

            // Inventory Canvas
            var invGo = new GameObject("Inventory_Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            invGo.transform.SetParent(uiRoot.transform);
            var invCanvas = invGo.GetComponent<Canvas>();
            invCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var invComp = invGo.AddComponent<InventoryUI>();
            invComp.EnsureUIHierarchy();

            // GameOver Canvas
            var goGo = new GameObject("GameOver_Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            goGo.transform.SetParent(uiRoot.transform);
            var goCanvas = goGo.GetComponent<Canvas>();
            goCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var goComp = goGo.AddComponent<GameOverUI>();
            goComp.EnsureUIHierarchy();

            // 13. Guardar escena
            EditorSceneManager.SaveScene(newScene, scenePath);
            Debug.Log($"[Build] Escena guardada exitosamente en {scenePath}");

            // 14. Registrar en EditorBuildSettings si no está
            EnsureSceneInBuildSettings(scenePath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateWall(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat) {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(parent);
            wall.transform.position = pos;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().material = mat;
        }

        private const string PREFAB_DIR = "Assets/Prefabs/Exploration";

        private static void EnsurePrefabDir() {
            if (!Directory.Exists(PREFAB_DIR)) {
                Directory.CreateDirectory(PREFAB_DIR);
            }
        }

        private static GameObject GetOrCreateDoorPrefab(string name, Material slabMat, bool isLocked, ItemId requiredKey, Vector3 openEuler) {
            EnsurePrefabDir();
            string path = $"{PREFAB_DIR}/{name}.prefab";
            AssetDatabase.DeleteAsset(path);

            var root = new GameObject(name);
            var doorComp = root.AddComponent<DoorInteractable>();
            doorComp.Configure(isLocked, requiredKey, openEuler, false);

            // Bisagra (Hinge / Pivot)
            var hinge = new GameObject("Hinge");
            hinge.transform.SetParent(root.transform, false);
            hinge.transform.localPosition = Vector3.zero;

            // Batiente de la puerta (DoorSlab)
            var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slab.name = "DoorSlab";
            slab.transform.SetParent(hinge.transform, false);
            slab.transform.localPosition = new Vector3(openEuler.y >= 0 ? 0.8f : -0.8f, 1.25f, 0f);
            slab.transform.localScale = new Vector3(1.6f, 2.5f, 0.12f);
            slab.GetComponent<Renderer>().material = slabMat;

            // Trigger para interacción suave y amplia en el centro de la puerta
            var trig = root.AddComponent<BoxCollider>();
            trig.isTrigger = true;
            trig.center = slab.transform.localPosition;
            trig.size = new Vector3(2.4f, 2.5f, 1.8f);

            // Jambas decorativas del marco
            var jambL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            jambL.name = "Jamb_Left";
            jambL.transform.SetParent(root.transform, false);
            jambL.transform.localPosition = new Vector3(-0.85f, 1.25f, 0f);
            jambL.transform.localScale = new Vector3(0.12f, 2.5f, 0.2f);
            jambL.GetComponent<Renderer>().material = slabMat;
            Object.DestroyImmediate(jambL.GetComponent<Collider>());

            var jambR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            jambR.name = "Jamb_Right";
            jambR.transform.SetParent(root.transform, false);
            jambR.transform.localPosition = new Vector3(0.85f, 1.25f, 0f);
            jambR.transform.localScale = new Vector3(0.12f, 2.5f, 0.2f);
            jambR.GetComponent<Renderer>().material = slabMat;
            Object.DestroyImmediate(jambR.GetComponent<Collider>());

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static GameObject GetOrCreateLevelTransitionDoorPrefab(string name, Material slabMat, Material darkMat, Vector3 targetSpawn) {
            EnsurePrefabDir();
            string path = $"{PREFAB_DIR}/{name}.prefab";
            AssetDatabase.DeleteAsset(path);

            var root = new GameObject(name);
            var doorComp = root.AddComponent<DoorInteractable>();
            doorComp.Configure(false, ItemId.None, new Vector3(0f, 90f, 0f), false, true, targetSpawn);

            // Bisagra (Hinge / Pivot)
            var hinge = new GameObject("Hinge");
            hinge.transform.SetParent(root.transform, false);
            hinge.transform.localPosition = Vector3.zero;

            // Batiente de la puerta (DoorSlab)
            var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slab.name = "DoorSlab";
            slab.transform.SetParent(hinge.transform, false);
            slab.transform.localPosition = new Vector3(0.8f, 1.25f, 0f);
            slab.transform.localScale = new Vector3(1.6f, 2.5f, 0.12f);
            slab.GetComponent<Renderer>().material = slabMat;

            // Fondo negro interior (DarkVoid)
            var darkVoid = GameObject.CreatePrimitive(PrimitiveType.Cube);
            darkVoid.name = "DarkVoid_Backdrop";
            darkVoid.transform.SetParent(root.transform, false);
            darkVoid.transform.localPosition = new Vector3(0f, 1.25f, 0.45f);
            darkVoid.transform.localScale = new Vector3(1.8f, 2.5f, 0.25f);
            darkVoid.GetComponent<Renderer>().material = darkMat;
            Object.DestroyImmediate(darkVoid.GetComponent<Collider>());

            // Trigger para interacción
            var trig = root.AddComponent<BoxCollider>();
            trig.isTrigger = true;
            trig.center = new Vector3(0f, 1.25f, 0f);
            trig.size = new Vector3(2.4f, 2.5f, 1.8f);

            // Jambas decorativas del marco
            var jambL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            jambL.name = "Jamb_Left";
            jambL.transform.SetParent(root.transform, false);
            jambL.transform.localPosition = new Vector3(-0.85f, 1.25f, 0f);
            jambL.transform.localScale = new Vector3(0.12f, 2.5f, 0.2f);
            jambL.GetComponent<Renderer>().material = slabMat;
            Object.DestroyImmediate(jambL.GetComponent<Collider>());

            var jambR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            jambR.name = "Jamb_Right";
            jambR.transform.SetParent(root.transform, false);
            jambR.transform.localPosition = new Vector3(0.85f, 1.25f, 0f);
            jambR.transform.localScale = new Vector3(0.12f, 2.5f, 0.2f);
            jambR.GetComponent<Renderer>().material = slabMat;
            Object.DestroyImmediate(jambR.GetComponent<Collider>());

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static GameObject GetOrCreatePushableBoxPrefab(string name, Material mat, float mass, float force, float maxDist) {
            EnsurePrefabDir();
            string path = $"{PREFAB_DIR}/{name}.prefab";
            AssetDatabase.DeleteAsset(path);

            var root = GameObject.CreatePrimitive(PrimitiveType.Cube);
            root.name = name;
            root.transform.localScale = new Vector3(1.3f, 0.8f, 1.3f);
            root.GetComponent<Renderer>().material = mat;

            var rb = root.AddComponent<Rigidbody>();
            rb.mass = mass;
            rb.linearDamping = 8f;
            rb.angularDamping = 10f;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = true;

            var push = root.AddComponent<PushableBox>();
            push.Configure(force, mass, 8f, maxDist);

            // Peldaño escalón (step) para trepar encima, desactivado por defecto
            var step = GameObject.CreatePrimitive(PrimitiveType.Cube);
            step.name = "Step_Ledge";
            step.transform.SetParent(root.transform, false);
            step.transform.localPosition = new Vector3(0f, -0.2f, -0.7f);
            step.transform.localScale = new Vector3(1.3f, 0.4f, 0.4f);
            step.GetComponent<Renderer>().material = mat;
            step.SetActive(false); // Inactivo hasta que la caja se bloquee en el destino final rojo

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static GameObject CreateDestinationSocket(Transform parent, string name, BoxDestinationType type, Vector3 position, Material mat) {
            var socketGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            socketGo.name = name;
            socketGo.transform.SetParent(parent, false);
            socketGo.transform.position = position;
            socketGo.transform.localScale = new Vector3(1.5f, 0.04f, 1.5f);
            socketGo.GetComponent<Renderer>().material = mat;

            var col = socketGo.GetComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(1.2f, 8f, 1.2f);

            var point = socketGo.AddComponent<BoxDestinationPoint>();
            point.Configure(type, 0.8f);

            return socketGo;
        }

        private static GameObject GetOrCreateLadderPrefab(string name, Material mat, float height) {
            EnsurePrefabDir();
            string path = $"{PREFAB_DIR}/{name}.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null) return existing;

            var root = new GameObject(name);
            var col = root.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.center = new Vector3(0f, height * 0.5f, 0f);
            col.size = new Vector3(1.2f, height, 0.8f);
            root.AddComponent<LadderZone>();

            var poleL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            poleL.name = "PoleL";
            poleL.transform.SetParent(root.transform, false);
            poleL.transform.localPosition = new Vector3(-0.4f, height * 0.5f, 0f);
            poleL.transform.localScale = new Vector3(0.08f, height, 0.08f);
            poleL.GetComponent<Renderer>().material = mat;
            Object.DestroyImmediate(poleL.GetComponent<Collider>());

            var poleR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            poleR.name = "PoleR";
            poleR.transform.SetParent(root.transform, false);
            poleR.transform.localPosition = new Vector3(0.4f, height * 0.5f, 0f);
            poleR.transform.localScale = new Vector3(0.08f, height, 0.08f);
            poleR.GetComponent<Renderer>().material = mat;
            Object.DestroyImmediate(poleR.GetComponent<Collider>());

            int rungs = Mathf.Max(2, Mathf.FloorToInt(height / 0.4f));
            for (int r = 0; r < rungs; r++) {
                var rung = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rung.name = $"Rung_{r}";
                rung.transform.SetParent(root.transform, false);
                rung.transform.localPosition = new Vector3(0f, 0.2f + (r * 0.4f), 0f);
                rung.transform.localScale = new Vector3(0.8f, 0.05f, 0.05f);
                rung.GetComponent<Renderer>().material = mat;
                Object.DestroyImmediate(rung.GetComponent<Collider>());
            }

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static GameObject GetOrCreatePickupPrefab(string name, ItemId itemId, Color color) {
            EnsurePrefabDir();
            string path = $"{PREFAB_DIR}/{name}.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null) return existing;

            var root = GameObject.CreatePrimitive(PrimitiveType.Cube);
            root.name = name;
            root.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            root.GetComponent<Renderer>().material = CreateMaterial($"Mat_Pickup_{itemId}", color);

            var col = root.GetComponent<BoxCollider>();
            col.isTrigger = true;

            var pickup = root.AddComponent<ItemPickup>();
            pickup.Configure(itemId);

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static Material CreateMaterial(string name, Color color) {
            string dir = "Assets/Materials/Testing";
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string path = $"{dir}/{name}.mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null) {
                Shader standardShader = Shader.Find("Universal Render Pipeline/Lit");
                if (standardShader == null) standardShader = Shader.Find("Standard");
                mat = new Material(standardShader);
                mat.color = color;
                AssetDatabase.CreateAsset(mat, path);
            } else {
                mat.color = color;
                EditorUtility.SetDirty(mat);
            }
            return mat;
        }

        private static void EnsureSceneInBuildSettings(string scenePath) {
            var scenes = EditorBuildSettings.scenes;
            foreach (var s in scenes) {
                if (s.path == scenePath) return;
            }

            var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            // Insertar 01_Functional_Testing en el índice 1 (después de 00_Bootstrap)
            int targetIndex = 1;
            int offset = 0;
            for (int i = 0; i < newScenes.Length; i++) {
                if (i == targetIndex) {
                    newScenes[i] = new EditorBuildSettingsScene(scenePath, true);
                    offset = 1;
                } else {
                    newScenes[i] = scenes[i - offset];
                }
            }
            EditorBuildSettings.scenes = newScenes;
            Debug.Log($"[Build] Registrada escena {scenePath} en EditorBuildSettings.");
        }
    }
}
#endif
