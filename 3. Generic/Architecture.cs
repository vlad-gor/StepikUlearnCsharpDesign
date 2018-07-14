using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots
{
    public interface RobotAI<out T>
    {
        T GetCommand();
    }

    public class ShooterAI : RobotAI<ShooterCommand>
    {
        int counter = 1;

        public ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    public class BuilderAI : RobotAI<BuilderCommand>
    {
        int counter = 1;

        public BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface Device<in T>
    {
        string ExecuteCommand(T command);
    }

    public class Mover : Device<IMoveCommand>
    {
        public string ExecuteCommand(IMoveCommand _command)
        {
            if (_command == null)
                throw new ArgumentException();
            return $"MOV {_command.Destination.X}, {_command.Destination.Y}";
        }
    }

    public class Robot
    {
        RobotAI<object> ai;
        Device<IMoveCommand> device;

        public Robot(RobotAI<object> ai, Device<IMoveCommand> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
             for (int i=0;i<steps;i++)
             {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                yield return device.ExecuteCommand(command as IMoveCommand);
            }
        }

        public static Robot Create(RobotAI<object> ai, Device<IMoveCommand> executor)
        {
            return new Robot(ai, executor);
        }
    }   
}