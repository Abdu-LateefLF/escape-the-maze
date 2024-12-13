using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseDoor1 : MonoBehaviour
	{

        public Animator openandclose1;
        public bool open;
        public AudioSource openSound;
        public AudioSource closeSound;

        void Start()
        {
            open = false;
        }

        public void Interact()
        {
            if (open == false)
            {
                StartCoroutine(opening());
            }
            else
            {
                StartCoroutine(closing());
            }
        }

        IEnumerator opening()
		{
			print("you are opening the door");
			openandclose1.Play("Opening 1");
            openSound.Play();
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		public IEnumerator closing()
		{
			print("you are closing the door");
			openandclose1.Play("Closing 1");
            closeSound.Play();
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}