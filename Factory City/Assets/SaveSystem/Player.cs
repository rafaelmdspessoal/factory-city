using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour, ISaveable
{
    public Transform playerCapsule;
    public Transform playerCamera;
    public Transform followCamera;

    public void PopulateSaveData(SaveData saveData)
    {
        saveData.playerData.playerCapsulePosition = playerCapsule.position;
        saveData.playerData.playerCapsuleRotation = playerCapsule.localRotation;

        saveData.playerData.playerCameraPosition = playerCamera.position;
        saveData.playerData.playerCameraRotation = playerCamera.localRotation;

        saveData.playerData.followCameraPosition = followCamera.position;
        saveData.playerData.followCameraRotation = followCamera.localRotation;
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        playerCapsule.position = saveData.playerData.playerCapsulePosition;
        playerCapsule.localRotation = saveData.playerData.playerCapsuleRotation;

        playerCamera.position = saveData.playerData.playerCameraPosition;
        playerCamera.localRotation = saveData.playerData.playerCameraRotation;

        followCamera.position = saveData.playerData.followCameraPosition;
        followCamera.localRotation = saveData.playerData.followCameraRotation;
    }
}