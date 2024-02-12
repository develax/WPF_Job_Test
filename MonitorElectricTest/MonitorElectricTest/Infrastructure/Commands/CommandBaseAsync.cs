using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.Infrastructure.Commands
{
    public abstract class CommandBaseAsync : CommandBase
    {
        public override async void Execute(object parameter)
        {
            await Execute_Async(parameter);
        }

        public abstract Task Execute_Async(object parameter);
    }
}
