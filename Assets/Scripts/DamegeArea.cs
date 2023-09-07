using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///…À∫¶œ›⁄Âœ‡πÿ
/// </summary>
public class Damege : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController pc=other.GetComponent<PlayerController>();
        if(pc != null )
        {
            pc.ChangeHealth(-1);
        }
    }
}
