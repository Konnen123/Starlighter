using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
   public NPC npc;

   private bool isTalking = false;
   private float distance;
   private int currentResponseTracker=0;


   [SerializeField] private float minDistance;
   [SerializeField] private TextMeshProUGUI npcDialogue;
   [SerializeField] private GameObject player,dialogueUI;
   [SerializeField] private GameObject playerDialoguesHolder,playerOptions;
   
   private void Start()
   {
      dialogueUI.SetActive(false);
   }

   private void OnMouseOver()
   {
    
      distance = Vector3.Distance(player.transform.position, transform.position);
      if (distance > minDistance)
      {
         EndDialogue();
         return;
      }

      if (Input.GetAxis("Mouse ScrollWheel")<0)
      {
         currentResponseTracker++;
         currentResponseTracker =  Mathf.Clamp(currentResponseTracker, 0, npc.playerDialogue.Length-1);

         ResetAlphaImage();
      }
      else if (Input.GetAxis("Mouse ScrollWheel")>0)
      {
         currentResponseTracker--;
         currentResponseTracker =  Mathf.Clamp(currentResponseTracker, 0, npc.playerDialogue.Length);
         
       ResetAlphaImage();
      }
      else if(Input.GetKeyDown(KeyCode.Return))
      {
         npcDialogue.text = npc.npcDialogue[currentResponseTracker+1];
      }

      if (!isTalking)
      {
         StartConversation();
      }
      
   }

   private void OnMouseExit()
   {
      EndDialogue();
   }

   private void StartConversation()
   {
    
      isTalking = true;
      for (int i = 0; i < npc.playerDialogue.Length; i++)
      {
      GameObject playerDialogues =Instantiate(playerOptions, playerDialoguesHolder.transform);
      if (i != 0)
      {
         Image image = playerDialogues.GetComponent<Image>();
         image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
      }

      playerDialogues.GetComponentInChildren<TextMeshProUGUI>().text = npc.playerDialogue[i];
      }
      currentResponseTracker = 0;
      dialogueUI.SetActive(true);

      npcDialogue.text = npc.npcDialogue[0];
   }

   private void EndDialogue()
   {
      
      isTalking = false;
      for (int i = 0; i < playerDialoguesHolder.transform.childCount; i++)
         Destroy(playerDialoguesHolder.transform.GetChild(i).gameObject);
      
      dialogueUI.SetActive(false);
      
   }

   void ResetAlphaImage()
   {
      for (int i = 0; i < npc.playerDialogue.Length; i++)
      {
         Image image = playerDialoguesHolder.transform.GetChild(i).GetComponent<Image>();
         if(i!=currentResponseTracker)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
         else
            image.color = new Color(image.color.r, image.color.g, image.color.b, .2f);
      }
   }

 
}
