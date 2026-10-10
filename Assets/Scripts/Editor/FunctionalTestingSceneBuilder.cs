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
            Material matKeyDoor = CreateMaterial("Mat_Test_KeyDoor", new Color(0.6f, 0.15f, 0.15f));
            Material matLadder = CreateMaterial("Mat_Test_Ladder", new Color(0.9f, 0.75f, 0.2f));

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

            // Barandillas del mezzanine
            CreateWall(verticalRoot.transform, "Railing_North", new Vector3(0f, 3.5f, 12f), new Vector3(16f, 1f, 0.2f), matWall);
            CreateWall(verticalRoot.transform, "Railing_West", new Vector3(-8f, 3.5f, 8f), new Vector3(0.2f, 1f, 8f), matWall);
            CreateWall(verticalRoot.transform, "Railing_East", new Vector3(8f, 3.5f, 8f), new Vector3(0.2f, 1f, 8f), matWall);
            // Barandillas del mezzanine con aperturas en X = -3 (escaleras) y X = 3 (rampa)
            CreateWall(verticalRoot.transform, "Railing_North", new Vector3(0f, 3.5f, 12f), new Vector3(16f, 1f, 0.2f), matWall);
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

            // Escala vertical tipo marinero / escalera de mano (Ladder) en X = 7.5, sube de Y=0 a Y=3.5
            var ladderGo = new GameObject("Ladder_Vertical");
            ladderGo.transform.SetParent(verticalRoot.transform);
            ladderGo.transform.position = new Vector3(7.5f, 1.5f, 3.8f);

            var ladderCol = ladderGo.AddComponent<BoxCollider>();
            ladderCol.isTrigger = true;
            ladderCol.size = new Vector3(1.2f, 3.5f, 0.8f);
            ladderGo.AddComponent<LadderZone>();

            // Visual de la escalera (postes y peldaños)
            var poleL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            poleL.name = "PoleL";
            poleL.transform.SetParent(ladderGo.transform);
            poleL.transform.localPosition = new Vector3(-0.4f, 0f, 0f);
            poleL.transform.localScale = new Vector3(0.08f, 3.2f, 0.08f);
            poleL.GetComponent<Renderer>().material = matLadder;

            var poleR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            poleR.name = "PoleR";
            poleR.transform.SetParent(ladderGo.transform);
            poleR.transform.localPosition = new Vector3(0.4f, 0f, 0f);
            poleR.transform.localScale = new Vector3(0.08f, 3.2f, 0.08f);
            poleR.GetComponent<Renderer>().material = matLadder;

            for (int r = 0; r < 8; r++) {
                var rung = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rung.name = $"Rung_{r}";
                rung.transform.SetParent(ladderGo.transform);
                rung.transform.localPosition = new Vector3(0f, -1.3f + (r * 0.4f), 0f);
                rung.transform.localScale = new Vector3(0.8f, 0.05f, 0.05f);
                rung.GetComponent<Renderer>().material = matLadder;
            }

            // --- 5. PUERTAS INTERACTIVAS (IInteractable) ---
            var doorsRoot = new GameObject("[=== INTERACTIVE DOORS ===]");

            // Puerta 1: Puerta estándar desbloqueada (Hueco X = -3, Z = 0)
            var door1Frame = new GameObject("Door_Standard_Unlocked");
            door1Frame.transform.SetParent(doorsRoot.transform);
            door1Frame.transform.position = new Vector3(-3.8f, 0f, 0f); // Pivote en la jamba

            var door1Slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door1Slab.name = "DoorSlab";
            door1Slab.transform.SetParent(door1Frame.transform);
            door1Slab.transform.localPosition = new Vector3(0.8f, 1.25f, 0f);
            door1Slab.transform.localScale = new Vector3(1.6f, 2.5f, 0.12f);
            door1Slab.GetComponent<Renderer>().material = matDoor;

            var door1Comp = door1Frame.AddComponent<DoorInteractable>();
            door1Slab.AddComponent<BoxCollider>(); // Asegurar collider para interactuar
            var door1Proxy = door1Slab.AddComponent<DoorInteractionProxy>();
            door1Proxy.TargetDoor = door1Comp;

            // Puerta 2: Puerta con cerrojo de seguridad (Requiere KeyCard_Level1, Hueco X = 3, Z = 0)
            var door2Frame = new GameObject("Door_Security_Locked");
            door2Frame.transform.SetParent(doorsRoot.transform);
            door2Frame.transform.position = new Vector3(3.8f, 0f, 0f); // Pivote en la jamba

            var door2Slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door2Slab.name = "DoorSlab_Locked";
            door2Slab.transform.SetParent(door2Frame.transform);
            door2Slab.transform.localPosition = new Vector3(-0.8f, 1.25f, 0f);
            door2Slab.transform.localScale = new Vector3(1.6f, 2.5f, 0.12f);
            door2Slab.GetComponent<Renderer>().material = matKeyDoor;

            var door2Comp = door2Frame.AddComponent<DoorInteractable>();
            door2Comp.Configure(true, ItemId.KeyCard_Level1, new Vector3(0f, -90f, 0f), false);

            door2Slab.AddComponent<BoxCollider>();
            var door2Proxy = door2Slab.AddComponent<DoorInteractionProxy>();
            door2Proxy.TargetDoor = door2Comp;

            // --- 6. CAJAS EMPUJABLES (PushableBox) ---
            var cratesRoot = new GameObject("[=== PUSHABLE BOXES ===]");

            // Caja 1 (En el pasillo sur, obstaculizando el acceso hacia la puerta)
            CreatePushableBox(cratesRoot.transform, "PushableCrate_1", new Vector3(-3f, 0.65f, -4f), new Vector3(1.3f, 1.3f, 1.3f), matCrate1, 20f, 2.8f);

            // Caja 2 (Otra caja empujable en el área de puzles)
            CreatePushableBox(cratesRoot.transform, "PushableCrate_2", new Vector3(-1f, 0.65f, -6f), new Vector3(1.3f, 1.3f, 1.3f), matCrate2, 25f, 2.4f);

            // Caja 3 (En el pasillo este)
            CreatePushableBox(cratesRoot.transform, "PushableCrate_3", new Vector3(5f, 0.65f, -4f), new Vector3(1.3f, 1.3f, 1.3f), matCrate1, 18f, 3.0f);

            // --- 7. OBJETOS EN EL SUELO (PICKUPS) ---
            var pickupsRoot = new GameObject("[=== PICKUPS ===]");

            // Green Herb en el área sur
            CreatePickup(pickupsRoot.transform, "Pickup_GreenHerb", ItemId.GreenHerb, new Vector3(-6f, 0.3f, -4f), Color.green);

            // First Aid Med en el área este
            CreatePickup(pickupsRoot.transform, "Pickup_FirstAidMed", ItemId.FirstAidMed, new Vector3(8f, 0.3f, -6f), Color.cyan);

            // KeyCard_Level1 EN EL SEGUNDO PISO (Mezzanine a Y = 3)
            // Obliga al jugador a usar las escaleras / verticalidad para conseguir la tarjeta que abre la puerta 2!
            CreatePickup(pickupsRoot.transform, "Pickup_KeyCard_Level1", ItemId.KeyCard_Level1, new Vector3(0f, 3.3f, 9.5f), Color.yellow);

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

        private static void CreatePushableBox(Transform parent, string name, Vector3 pos, Vector3 size, Material mat, float mass, float force) {
            var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.name = name;
            box.transform.SetParent(parent);
            box.transform.position = pos;
            box.transform.localScale = size;
            box.GetComponent<Renderer>().material = mat;

            var rb = box.AddComponent<Rigidbody>();
            rb.mass = mass;
            rb.linearDamping = 6f;
            rb.angularDamping = 10f;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

            var pushComp = box.AddComponent<PushableBox>();
            pushComp.Configure(force, mass, 6f);
        }

        private static void CreatePickup(Transform parent, string name, ItemId itemId, Vector3 pos, Color color) {
            var pickup = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pickup.name = name;
            pickup.transform.SetParent(parent);
            pickup.transform.position = pos;
            pickup.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            pickup.GetComponent<Renderer>().material = CreateMaterial($"Mat_Pickup_{itemId}", color);

            var col = pickup.GetComponent<BoxCollider>();
            col.isTrigger = true;

            var itemComp = pickup.AddComponent<ItemPickup>();
            itemComp.Configure(itemId);
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

    /// <summary>
    /// Proxy para dirigir la interacción del collider del batiente hacia el DoorInteractable raíz
    /// </summary>
    public class DoorInteractionProxy : MonoBehaviour, Game.Interfaces.IInteractable {
        public DoorInteractable TargetDoor;

        public void Interact(ExplorationController player) {
            if (TargetDoor != null) TargetDoor.Interact(player);
        }

        public string GetInteractionPrompt() {
            return TargetDoor != null ? TargetDoor.GetInteractionPrompt() : "Puerta";
        }
    }
}
#endif
