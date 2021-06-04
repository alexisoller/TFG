using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_ResetRoom : MonoBehaviour {

    public List<EnemyPosition> reset = new List<EnemyPosition>();
    public Room_PlayableArea playerInRoom;
    public GameObject mainCamera;
    public GameObject[] positionCameraOtherRoom;

    private bool cameraInPosition = false;
    private bool active;

    [System.Serializable]
    public class EnemyPosition {
        public GameObject originalPoint;
        public GameObject enemy;
    }

    private void Start() {
        if (reset.Count == 0) {
            active = false;
        } else {
            active = true;
        }
    }

    private void Update() {
        if (active) {
            if (!playerInRoom.playerIsInArea && !cameraInPosition) {
                for (int i = 0; i < positionCameraOtherRoom.Length; i++) {
                    if (mainCamera.transform.position == positionCameraOtherRoom[i].transform.position) {
                        cameraInPosition = true;
                    }
                }
            }

            if (!playerInRoom.playerIsInArea && cameraInPosition) {
                for (int i = 0; i < reset.Count; i++) {
                    reset[i].enemy.transform.position = reset[i].originalPoint.transform.position;
                    if (reset[i].enemy.GetComponent<Enemy_Small>()) {
                        reset[i].enemy.GetComponent<Enemy_Small>().currentHealth = reset[i].enemy.GetComponent<Enemy_Small>().healthPoints;
                    }

                    if (reset[i].enemy.GetComponent<Enemy_Medium>()) {
                        reset[i].enemy.GetComponent<Enemy_Medium>().currentHealth = reset[i].enemy.GetComponent<Enemy_Medium>().healthPoints;
                        reset[i].enemy.GetComponent<Enemy_Medium>().enemyVisualAspect.SetActive(true);
                        reset[i].enemy.GetComponent<Enemy_Medium>().shadowVisualAspect.SetActive(false);
                        reset[i].enemy.GetComponent<Enemy_Medium>().colliderEnemy.enabled = false;
                        reset[i].enemy.GetComponent<Enemy_Medium>().move = true;
                        reset[i].enemy.GetComponent<Enemy_Medium>().death = false;
                        reset[i].enemy.GetComponent<Enemy_Medium>().colliderEnemyTrigger.enabled = true;
                        reset[i].enemy.GetComponent<Enemy_Medium>().countDeath = 0;
                        reset[i].enemy.GetComponent<Enemy_Medium>().damageVisualAspect.SetActive(false);
                        if (reset[i].enemy.GetComponent<Enemy_Medium>().newEnemy.Count > 0){
                            for (int j = 0; j < reset[i].enemy.GetComponent<Enemy_Medium>().newEnemy.Count; j++) {
                                Destroy(reset[i].enemy.GetComponent<Enemy_Medium>().newEnemy[j].gameObject);
                                reset[i].enemy.GetComponent<Enemy_Medium>().newEnemy.RemoveAt(j);
                            }
                        }
                    }
                }
                cameraInPosition = false;
            }

            if (reset.Count == 0) {
                active = false;
            }
        }
    }
}
