using UnityEngine;

public class Hammer : MonoBehaviour
{
    public BoardRotator board;
    private void Start()
    {
        
    }
    SlotHandler slotHandler;
    public void PositionHammer(Transform Target,SlotHandler slot)
    {
        //Vector3 NewPosition = Target + new Vector3(-.3f, .7f, 0);
        gameObject.SetActive(true);
        slotHandler=slot;
        transform.transform.SetPositionAndRotation(Target.position, Target.rotation);
    }
    public void DeactiveMe()
    {
        gameObject.SetActive(false);

    }
    public void ActiveteaParticles()
    {
        slotHandler.ActivateDebris();
        board.StartShaking(); 
    }
}
