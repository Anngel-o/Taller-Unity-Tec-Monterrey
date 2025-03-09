using NUnit.Framework;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceType
    {
        Normal,
        Bad
    }
    public PieceType pieceType = PieceType.Normal;
    private AudioSource audioSource;
    public float offsetSeconds;
    public MeshRenderer meshRenderer;
    public Collider collider;
    public bool isLastPiece;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Función para desactivar todas las piezas dentro de la pieza principal
            // esto a través de desactivar sus colliders y renderers para no 
            // desactivar sus demás components como audio source y demás
            Collider[] childColliders = transform.parent.GetComponentsInChildren<Collider>();
            foreach (Collider childCollider in childColliders)
            {
                childCollider.enabled = false;
                childCollider.GetComponent<Renderer>().enabled = false;
            }

            if (audioSource != null)
            {
                audioSource.time = offsetSeconds;
                audioSource.pitch = 1.0f + GameManager.Instance.ComboCounter();
                audioSource.Play();
            }

            if (meshRenderer != null)
            {
                collider.enabled = false;
                if (pieceType == PieceType.Normal)
                    meshRenderer.enabled = false;
            }

            GameManager.Instance.Pop();
            if (isLastPiece)
            {
                GameManager.Instance.Win();
            }

            if (pieceType == PieceType.Bad)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
