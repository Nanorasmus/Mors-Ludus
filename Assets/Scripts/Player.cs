using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort ID { get; private set; }
    public bool IsLocal{ get; private set; }

    [SerializeField] private Transform headTilt;
    [SerializeField] private Animator animator;

    private string username;
    private Rigidbody rb;

    private void OnDestroy()
    {
        list.Remove(ID);
    }

    private void Move(Vector3 newPosition, Vector3 forward)
    {



        if (!IsLocal)
        {
            headTilt.forward = new Vector3(0, forward.y, forward.z);

            animator.SetFloat("X", (transform.InverseTransformPoint(newPosition)).x / Time.deltaTime);
            animator.SetFloat("Z", (transform.InverseTransformPoint(newPosition)).z / Time.deltaTime);
            animator.SetFloat("HeadLookAngle", 0.5f + (forward.y / 2));

            transform.forward = new Vector3(forward.x, 0, forward.z);
        }
        transform.position = newPosition;

    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.ID = id;
        player.username = username;
        player.rb = player.gameObject.GetComponent<Rigidbody>();

        list.Add(id, player);
    }

    [MessageHandler((ushort)ServertoClientID.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServertoClientID.playerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.Move(message.GetVector3(), message.GetVector3());
        }
    }
}
