using System.Collections.Generic;
using UnityEngine;

public class ParticleCustomer : MonoBehaviour
{
    public float xTimer = 0f, wait_Time;

    int randEVAL;

    ParticleSystem Emoji_Particle;
    [SerializeField] List<Sprite> Emojis;
    void Start()
    {
        xTimer = 0f; wait_Time = 6f;
        Emoji_Particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        xTimer += Time.deltaTime;
        if (xTimer > wait_Time)
        {
            randEVAL = Random.Range(0, Emojis.Count);
            Emoji_Particle.textureSheetAnimation.AddSprite(Emojis[randEVAL]);
            Emoji_Particle.Play();
            wait_Time = Random.Range(9, 12);
            Emoji_Particle.textureSheetAnimation.RemoveSprite(0);
            xTimer = 0f;
        }
    }
}
