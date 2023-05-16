using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Console;
using Example;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    public string input;
    public bool invokeComand;
    [TextArea]public string outputMessage;
    
    List<object> commands = new List<object>();
    

    void Start()
    {
        InitializeCommands();
    }

    void InitializeCommands()
    {
        commands.Add(new CommandData("hello_world", "hello_world", () => ExampleDataController.instance.HelloWorld()));
        commands.Add(new CommandData<string>("set_message", "set_message <message>", (message) => ExampleDataController.instance.SetMessage(message)));
        commands.Add(new CommandData<double>("set_number", "set_number <number>", (number) => ExampleDataController.instance.SetNumber(number)));
    }

    void Update()
    {
        if (invokeComand)
        {
            InvokeCommad();
            invokeComand = false;
        }
    }

    void InvokeCommad()
    {
        CommandHandler(input);
        input = "";
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
                    outputMessage = $"command invoke succesfully";
                }
                else if (command as CommandData<double> != null && double.TryParse(properties[1], out double value))
                {
                    (command as CommandData<double>).Invoke(value);
                    outputMessage = $"command invoke succesfully";
                }
                else if (command as CommandData<string> != null)
                {
                    (command as CommandData<string>).Invoke(properties[1]);
                    outputMessage = $"command invoke succesfully";
                }
                else
                {
                    outputMessage = $"wrong command, format is [{command.format}]";
                }
            }
            else
            {
                outputMessage = $"the command does not exist";
            }
        }
    }
}
