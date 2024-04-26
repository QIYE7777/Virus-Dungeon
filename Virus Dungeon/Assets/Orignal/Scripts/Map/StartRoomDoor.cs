using UnityEngine;
using DG.Tweening;

public class StartRoomDoor : MonoBehaviour
{
    public Transform door;
    public Vector3 doorOpenRotation;

    public Vector3 doorCloseRotation;

    public void OpenDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorOpenRotation, 1.1f).SetEase(Ease.OutBack);
    }

    public void CloseDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorCloseRotation, 1.1f).SetEase(Ease.InCubic);
    }
}
