using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace IsometricShooter3D
{
    public class HealthUI : MonoBehaviour
    {
        // Reference to text component.
        private Text _healthText;

        private void OnEnable()
        {
            this.AddObserver(OnCharacterDamagedNotification, CharacterModel.CharacterDamagedNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnCharacterDamagedNotification, CharacterModel.CharacterDamagedNotification);
        }

        private void Start()
        {
            _healthText = GetComponent<Text>();
        }

        // When damage is dealt to the player, we update the health.
        private void OnCharacterDamagedNotification (object sender, object args)
        {
            var damagedCharacter = sender as CharacterModel;

            // Ensure the damaged character is the player.
            if (damagedCharacter.GetComponent<PlayerModel>() != null)
            {
                var player = damagedCharacter.GetComponent<PlayerModel>();

                _healthText.text = "Health : " + Mathf.RoundToInt(player.Health);
            }
        }
    }
}