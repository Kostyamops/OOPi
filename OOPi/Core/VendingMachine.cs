using VendingMachineApp.Models;
using VendingMachineApp.Utils;

namespace VendingMachineApp.Core;

public class VendingMachine
{
    private List<Slot> _slots = new();
    private MoneyStorage _moneyStorage = new();

    private decimal _cashBalance = 0m;    // нал
    private decimal _cardBalance = 0m;    // карта
    private const string ADMIN_PASS = "admin";
    private readonly Random _rnd = new();

    public VendingMachine()
    {
        for (int i = 0; i < 9; i++) _slots.Add(new Slot());
    }

    public void SeedDemo()
    {
        _slots[0].SetProduct(new Product("Вода"), 35, 5);
        _slots[1].SetProduct(new Product("Сок"), 55, 3);
        _slots[2].SetProduct(new Product("Чипсы"), 70, 4);
        _slots[3].SetProduct(new Product("Шоколад"), 80, 2);
        _slots[4].SetProduct(new Product("Кофе"), 60, 6);
        _slots[5].SetProduct(new Product("Чай"), 40, 2);
        _slots[6].SetProduct(new Product("Батончик"), 50, 5);
        _slots[7].SetProduct(new Product("Орехи"), 90, 1);
        _slots[8].SetProduct(new Product("Лимонад"), 65, 4);

        _moneyStorage.TryAddMoney(10);
        _moneyStorage.TryAddMoney(50);
        _moneyStorage.TryAddMoney(100);
        _moneyStorage.TryAddMoney(500);
    }

    private decimal TotalUserBalance => _cashBalance + _cardBalance;

    public void DisplayMenu()
    {
        while (true)
        {
            ConsoleUI.Clear();
            Console.WriteLine("Меню");
            Console.WriteLine("------------------");
            Console.WriteLine($"Наличные: {_cashBalance}");
            Console.WriteLine($"Карта:    {_cardBalance}");
            Console.WriteLine($"Итого:    {TotalUserBalance}");
            Console.WriteLine();
            Console.WriteLine("1. Товары");
            Console.WriteLine("2. Пополнить");
            Console.WriteLine("3. Купить");
            Console.WriteLine("4. Сдача");
            Console.WriteLine("9. Админ");
            Console.WriteLine("0. Выход");
            Console.Write(">> ");
            var c = Console.ReadLine();
            switch (c)
            {
                case "1": ShowAllProducts(); break;
                case "2": AddFundsFlow(); break;
                case "3": BuyFlow(); break;
                case "4": ReturnChange(); break;
                case "9": EnterAdminMode(); break;
                case "0": return;
                default: ConsoleUI.Pause("Ошибка ввода."); break;
            }
        }
    }

    private void ShowAllProducts()
    {
        ConsoleUI.Clear();
        Console.WriteLine("Товары");
        Console.WriteLine("------------------");
        for (int i = 0; i < _slots.Count; i++)
        {
            var s = _slots[i];
            string name = s.IsEmpty() ? "ПУСТО" : s.Product!.Name;
            Console.WriteLine($"{i+1}. {name,-10} | Цена: {s.Price} | Кол-во: {s.Quantity}");
        }
        ConsoleUI.Pause();
    }

    private void AddFundsFlow()
    {
        bool again = true;
        while (again)
        {
            ConsoleUI.Clear();
            Console.WriteLine("Пополнение");
            Console.WriteLine("------------------");
            Console.WriteLine("1. Наличные");
            Console.WriteLine("2. Карта");
            Console.WriteLine("0. Назад");
            Console.Write(">> ");
            var m = Console.ReadLine();
            if (m == "0") break;

            if (m == "1")
            {
                Console.WriteLine("Номинал (1,2,5,10,50,100,200,500) или x.");
                Console.Write(">> ");
                var t = Console.ReadLine();
                if (t?.ToLower()=="x") { }
                else if (decimal.TryParse(t, out var denom))
                {
                    if (_moneyStorage.IsValidDenomination(denom))
                    {
                        _moneyStorage.TryAddMoney(denom);
                        _cashBalance += denom;
                        Console.WriteLine($"Принято: {denom}");
                    }
                    else
                    {
                        Console.WriteLine("Номинал не принят.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода.");
                }
            }
            else if (m == "2")
            {
                // 70% шанс ошибки
                bool fail = _rnd.NextDouble() < 0.7;
                if (fail)
                {
                    Console.WriteLine("Ошибка подключения к сети");
                    string msg = "*Наверное опять глушат...*";
                    foreach (char ch in msg)
                    {
                        Console.Write(ch);
                        Thread.Sleep(40);
                    }
                    Console.WriteLine();
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("Сумма пополнения (карта) или x.");
                    Console.Write(">> ");
                    var t = Console.ReadLine();
                    if (t?.ToLower()=="x") { }
                    else if (decimal.TryParse(t, out var amount) && amount > 0)
                    {
                        _cardBalance += amount;
                        Console.WriteLine($"Зачислено: {amount}");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Неизвестно.");
            }

            Console.Write("Ещё пополнение? (y/n): ");
            again = Console.ReadLine()?.ToLower() == "y";
        }
    }

    private void BuyFlow()
    {
        ConsoleUI.Clear();
        Console.Write("Номер слота (1-9) или 0: ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 0 || idx > 9)
        {
            ConsoleUI.Pause("Ошибка.");
            return;
        }
        if (idx == 0) return;
        PurchaseProduct(idx - 1);
    }

    private void DeductFromBalances(decimal amount)
    {
        if (_cashBalance >= amount)
        {
            _cashBalance -= amount;
            return;
        }
        decimal fromCash = _cashBalance;
        amount -= fromCash;
        _cashBalance = 0;
        _cardBalance -= amount;
        if (_cardBalance < 0) _cardBalance = 0;
    }

    public void PurchaseProduct(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _slots.Count)
        {
            ConsoleUI.Pause("Нет ячейки.");
            return;
        }

        var slot = _slots[slotIndex];
        if (slot.IsEmpty())
        {
            ConsoleUI.Pause("Пусто.");
            return;
        }

        if (TotalUserBalance < slot.Price)
        {
            ConsoleUI.Pause("Недостаточно средств.");
            return;
        }

        DeductFromBalances(slot.Price);
        slot.RemoveQuantity(1);
        ConsoleUI.Pause("Товар выдан.");
    }

    public void ReturnChange()
    {
        ConsoleUI.Clear();
        var total = TotalUserBalance;
        if (total <= 0)
        {
            ConsoleUI.Pause("Баланс пуст.");
            return;
        }

        var pack = _moneyStorage.GetChange(total);
        decimal given = pack.Sum(kv => kv.Key * kv.Value);

        if (given == 0)
        {
            ConsoleUI.Pause("Невозможно выдать.");
            return;
        }
        
        decimal remainingToSubtract = given;
        if (_cashBalance >= remainingToSubtract)
        {
            _cashBalance -= remainingToSubtract;
            remainingToSubtract = 0;
        }
        else
        {
            remainingToSubtract -= _cashBalance;
            _cashBalance = 0;
            if (_cardBalance >= remainingToSubtract)
            {
                _cardBalance -= remainingToSubtract;
                remainingToSubtract = 0;
            }
            else
            {
                _cardBalance = 0;
            }
        }

        Console.WriteLine("Выдано:");
        foreach (var kv in pack.OrderByDescending(k=>k.Key))
            Console.WriteLine($"{kv.Key} x {kv.Value}");
        Console.WriteLine($"Сумма: {given}");

        decimal still = TotalUserBalance;
        if (still > 0)
            Console.WriteLine($"Остаток: {still}");

        ConsoleUI.Pause();
    }

    private void EnterAdminMode()
    {
        ConsoleUI.Clear();
        Console.Write("Пароль: ");
        var pass = Console.ReadLine();
        if (pass != ADMIN_PASS)
        {
            ConsoleUI.Pause("Отказано.");
            return;
        }

        bool run = true;
        while (run)
        {
            ConsoleUI.Clear();
            Console.WriteLine("Админ");
            Console.WriteLine("------------------");
            Console.WriteLine("1. Слоты");
            Console.WriteLine("2. Настроить слот");
            Console.WriteLine("3. Очистить слот");
            Console.WriteLine("4. Инкассация");
            Console.WriteLine("0. Назад");
            Console.Write(">> ");
            var c = Console.ReadLine();
            switch (c)
            {
                case "1": AdminShowSlots(); break;
                case "2": AdminSetSlot(); break;
                case "3": AdminClearSlot(); break;
                case "4": AdminIncassation(); break;
                case "0": run = false; break;
                default: ConsoleUI.Pause("Ошибка."); break;
            }
        }
    }

    private void AdminShowSlots()
    {
        ConsoleUI.Clear();
        Console.WriteLine("Слоты");
        Console.WriteLine("------------------");
        for (int i=0;i<_slots.Count;i++)
        {
            var s = _slots[i];
            string name = s.IsEmpty() ? "ПУСТО" : s.Product!.Name;
            Console.WriteLine($"{i+1}. {name} | Цена {s.Price} | Кол-во {s.Quantity}");
        }
        Console.WriteLine($"Наличных внутри: {_moneyStorage.TotalCash()}");
        Console.WriteLine($"Карточный баланс (внесённый): {_cardBalance}");
        ConsoleUI.Pause();
    }

    private void AdminSetSlot()
    {
        ConsoleUI.Clear();
        Console.Write("Номер слота (1-9): ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx <1 || idx>9)
        {
            ConsoleUI.Pause("Ошибка.");
            return;
        }
        var s = _slots[idx-1];
        Console.Write("Название: ");
        var name = Console.ReadLine() ?? "Безымянный";
        Console.Write("Цена: ");
        if (!decimal.TryParse(Console.ReadLine(), out var price) || price<=0)
        {
            ConsoleUI.Pause("Ошибка цены.");
            return;
        }
        Console.Write("Количество: ");
        if (!int.TryParse(Console.ReadLine(), out var qty) || qty<0)
        {
            ConsoleUI.Pause("Ошибка количества.");
            return;
        }
        s.SetProduct(new Product(name), price, qty);
        ConsoleUI.Pause("Готово.");
    }

    private void AdminClearSlot()
    {
        ConsoleUI.Clear();
        Console.Write("Номер слота (1-9): ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx <1 || idx>9)
        {
            ConsoleUI.Pause("Ошибка.");
            return;
        }
        var s = _slots[idx-1];
        s.SetProduct(new Product(s.Product?.Name ?? ""), s.Price, 0);
        ConsoleUI.Pause("Очищено.");
    }

    private void AdminIncassation()
    {
        ConsoleUI.Clear();
        Console.WriteLine("Инкассация");
        Console.WriteLine("------------------");
        Console.WriteLine("1. Нал");
        Console.WriteLine("2. Карта");
        Console.WriteLine("0. Назад");
        Console.Write(">> ");
        var c = Console.ReadLine();
        if (c == "0") return;

        Console.Write("Сумма: ");
        if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
        {
            ConsoleUI.Pause("Ошибка суммы.");
            return;
        }

        if (c == "1")
        {
            var pack = _moneyStorage.TryExtractExact(amount);
            if (pack == null)
            {
                ConsoleUI.Pause("Невозможно собрать указанную сумму номиналами.");
                return;
            }
            Console.WriteLine("Выдано (нал):");
            foreach (var kv in pack.OrderByDescending(k=>k.Key))
                Console.WriteLine($"{kv.Key} x {kv.Value}");
            Console.WriteLine($"Сумма: {amount}");
            
            decimal reduce = Math.Min(_cashBalance, amount);
            _cashBalance -= reduce;

            ConsoleUI.Pause();
        }
        else if (c == "2")
        {
            if (_cardBalance < amount)
            {
                ConsoleUI.Pause("Недостаточно карточных средств.");
                return;
            }
            _cardBalance -= amount;
            Console.WriteLine($"Списано с карточного баланса: {amount}");
            ConsoleUI.Pause();
        }
        else
        {
            ConsoleUI.Pause("Неизвестный выбор.");
        }
    }
}