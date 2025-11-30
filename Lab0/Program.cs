using VendingMachineApp.Core;

namespace VendingMachineApp;

class Program
{
    static void Main()
    {
        var vm = new VendingMachine();
        vm.SeedDemo();
        vm.DisplayMenu();
    }
}