using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
	public class Drawer_Pull_X : MonoBehaviour
	{
		public Animator pull_01;
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
			pull_01.Play("openpull_01");
			openSound.Play();
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		public IEnumerator closing()
		{
			pull_01.Play("closepush_01");
			closeSound.Play();	
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}