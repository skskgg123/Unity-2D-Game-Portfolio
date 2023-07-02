using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] bool self_target;
    [SerializeField] SpriteRenderer sprite_target;
    [SerializeField] Sprite[] sprite_ani;
    [SerializeField] float time; // �ܺ����� �ð���Ұ� ������ ����
    [SerializeField] float waitTime;

    [SerializeField] float delayTime;
    int reset_count = 0;

    public bool isExplosion = false;

    public AudioClip clip;

    private void Start()
    {
        StartEffect();
        
    }

    //
    private void OnTimerEnd()
    {
        if (reset_count < sprite_ani.Length)
        {
            sprite_target.sprite = sprite_ani[reset_count];
            reset_count++;  
        }
        else
        {
            reset_count = 0;
            sprite_target.sprite = sprite_ani[reset_count];
            reset_count++;
            //SoundManager.instance.SFXPlay("Explosion", clip);
        }
        if (reset_count == sprite_ani.Length)
        {
            if (clip != null)
            {
                SoundManager.instance.SFXPlay("Explosion", clip);
            }
            Invoke("OnTimerEnd", time + waitTime); // �ܺ����� �ð���Ұ� ������ ����
        }
        else
        {
            Invoke("OnTimerEnd", time); // �ܺ����� �ð���Ұ� ������ ����
        }
    }

    public void StartEffect()
    {
        
        if (sprite_ani.Length == 0)
        {
            //Log.Warning($"Empty ani array in {gameObject.name}");
            enabled = false;
            return;
        }

        if (self_target)
        {
            sprite_target = GetComponent<SpriteRenderer>();
        }

        Invoke("OnTimerEnd", delayTime); // �ܺ����� �ð���Ұ� ������ ����
    }

}