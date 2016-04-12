using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace EndlessTorusRunner3D
{
    [RequireComponent(typeof(Text))]
    public class TrackScore : MonoBehaviour
    {
        #region Properties
        // References.
        [SerializeField]
        GameController controller;

        [SerializeField]
        Text scoreText;

        // Game Score
        int score;
        #endregion

        void Awake()
        {
            scoreText = GetComponent<Text>();
        }

        void Update()
        {
            score = controller.Score;
            scoreText.text = "Score : " + score;
        }
    }
}
