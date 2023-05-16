using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Example
{

    public class ExampleDataController : MonoBehaviour
    {
        public static ExampleDataController instance;
        
        [SerializeField] string message;
        [SerializeField] double number;


        void Awake()
        {
            instance = this;
            
            Initialize();
        }

        void Initialize()
        {
            message = "";
            number = 0;
        }
        
        
        public void HelloWorld()
        {
            Debug.Log("Hello World");
        }

        public void SetMessage(string message)
        {
            this.message = message;
        }

        public void SetNumber(double number)
        {
            this.number = number;
        }

    }
}
