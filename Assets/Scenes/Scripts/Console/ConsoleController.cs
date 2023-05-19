using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using Console;
using Example;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    public TMP_InputField inputField;
    
    [SerializeField] Transform content;
    [SerializeField] GameObject messagePrefab;

    [SerializeField] List<TMP_Text> commandObjects;
    List<object> commands = new List<object>();


    void Awake()
    {
        Application.logMessageReceived += GetLogMessage;
    }
    
    void OnDestroy()
    {
        Application.logMessageReceived -= GetLogMessage;
    }

    void Start()
    {
        InitializeCommands();
    }

    void InitializeCommands()
    {
        commands.Add(new CommandData("hello_world", "hello_world", () => ExampleDataController.instance.HelloWorld()));
        commands.Add(new CommandData<string>("set_message", "set_message <message>", (message) => ExampleDataController.instance.SetMessage(message)));
        commands.Add(new CommandData<double>("set_number", "set_number <number>", (number) => ExampleDataController.instance.SetNumber(number)));
        commands.Add(new CommandData("throw_exception", "throw_exception", () => ExampleDataController.instance.ThrowException()));
    }

    
    void GetLogMessage(string value, string stacktrace, LogType type)
    {
        ShowLogMessage(value, type);
    }

    public void InvokeCommad()
    {
        string value = inputField.text;
        
        inputField.DeactivateInputField();
        inputField.text = "";
        
        ShowLogMessage(value, LogType.Log);
        CommandHandler(value);
    }

    void ShowLogMessage(string value, LogType type)
    {
        string messageTime = $"[{System.DateTime.Now.Hour}:{System.DateTime.Now.Minute}:{System.DateTime.Now.Second}]";
        string messageInformation = value;
        string messageType = "";
        if(type == LogType.Exception)
            messageType = "<color=red>";
        
        string message = $"{messageTime}  {messageType}{messageInformation}</color>";

        GameObject messageObject = Instantiate(messagePrefab, content);
        TMP_Text commandText = messageObject.GetComponent<TMP_Text>();
        commandText.text = message;
        commandObjects.Add(commandText);

        if (commandObjects.Count > 9)
        {
            GameObject firstCommad = commandObjects[0].gameObject;
            commandObjects.RemoveAt(0);
            Destroy(firstCommad);
        }
    }

    void CommandHandler(string inputValue)
    {
        string[] properties = inputValue.Split(" ");

        foreach (CommandBaseData command in commands)
        {
            if (inputValue.Contains(command.id))
            {
                if (command as CommandData != null && properties.Length <= 1 )
                {
                    (command as CommandData).Invoke();
                    return;
                }
                else if (command as CommandData<double> != null && properties.Length > 1 && double.TryParse(properties[1], out double value))
                {
                    (command as CommandData<double>).Invoke(value);
                    return;
                }
                else if (command as CommandData<string> != null && properties.Length > 1)
                {
                    (command as CommandData<string>).Invoke(properties[1]);
                    return;
                }
                else
                {
                    Debug.Log($"<color=orange>wrong command, format is [{command.format}]");
                    return;
                }
            }
        }
        
        Debug.Log($"<color=orange>the command does not exist");
    }

    public void FindSimiliarCommend()
    {
        string value = inputField.text;

        foreach (var command in commands)
        {
            CommandBaseData commandData = command as CommandBaseData;
            if ( commandData.id.Contains(value) || value.Contains(commandData.id))
            {
                // print($"command id - {commandData.id}, your phrase - {value}");
            }
        }
    }
}
