using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Transform player;
    public Text scoreText;

    // Update is called once per frame
    // Uppfærsla er kölluð einu sinni í hvern ramma
    void Update()
    {
        scoreText.text = player.position.z.ToString("0");
    }
}
