using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IsometricShooter3D
{
    public class HealthUI : MonoBehaviour
    {
        // Reference to text component.
        Text healthText;

        void OnEnable()
        {
            this.AddObserver(OnCharacterDamagedNotification, CharacterModel.CharacterDamagedNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnCharacterDamagedNotification, CharacterModel.CharacterDamagedNotification);
        }

        void Start()
        {
            healthText = GetComponent<Text>();
        }

        // When damage is dealt to the player, we update the health.
        void OnCharacterDamagedNotification (object sender, object args)
        {
            CharacterModel damagedCharacter = sender as CharacterModel;

            // Ensure the damaged character is the player.
            if (damagedCharacter.GetComponent<PlayerModel>() != null)
            {
                PlayerModel player = damagedCharacter.GetComponent<PlayerModel>();

                healthText.text = "Health : " + Mathf.RoundToInt(player.Health);
            }
        }

        void Update()
        {

        }
    }
}