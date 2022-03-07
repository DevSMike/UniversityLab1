using System;
using System.Collections.Generic;
using System.IO;

namespace Lab3
{

    // класс с пациентами
   class Patients: IDisposable
    {
        //поля по заданию
        public string lastname;
        public int age;
        public string sex;
        public string livingplace;
        public string diagnosis;
        //реализация метода Dispose()
        public void Dispose()
        { 
            // задача метода Dispose() состоит в том, чтобы освободить неуправляемые ресурсы и указать, что метод завершения
            // не должен выполняться,
            //если он задан
            GC.SuppressFinalize(this);
            
        }
        ~Patients()
        {
            Console.WriteLine(" Объект класса Patients уничтожен.");
        }
        
    }
    //Тк во второй лабе коллекция была очередь, в этой лабе используется очередь.
    class Program
    {
        
        //функция меню, возвращает выбранный вариант 
        static int Menu()
        {
            Console.Clear();
            int chosen = -1;
            Console.WriteLine("Выберите пункт меню: ");
            Console.WriteLine("1. Добавить пациента");
            Console.WriteLine("2. Удалить пациента под номером");
            Console.WriteLine("3. Вывести всех пациентов");
            Console.WriteLine("4. Изменить пациента под номером");
            Console.WriteLine("5. Количество иногородних пациентов, прибывших в клинику");
            Console.WriteLine("6. Вывести сведения о пациентах пенсионного возраста");
            Console.WriteLine("         /////ЗАДАНИЕ К ТРЕТЕЙ ЛАБЕ /////");
            Console.WriteLine("7. Удалить объект из коллекции с дальнейшим уничтожением");
            Console.WriteLine("8. Вызов метода Dispose() для всех объектов коллекции");
            Console.WriteLine("9. Вызвать сборщик мусора");
            Console.WriteLine("10.Вывод поколений для всех объектов коллекции");
            Console.WriteLine("0. Выход");
            chosen = Convert.ToInt32(Console.ReadLine());

            return chosen;
        }
        static void Main(string[] args)
        {
            //задаем начальное имя файла с пациентами
            string filename = "patients.dat";
            // создаём очередь
            Queue<Patients> qpatients = new Queue<Patients>();
            Console.WriteLine("Введите имя файла:");
            filename = Console.ReadLine();
            //если файл существует
            if (File.Exists(filename))
            {
                // считывание информации с бинарного файла
                using (BinaryReader sr = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    while (sr.PeekChar() != -1)
                    {
                        Patients patients = new Patients();
                        patients.lastname = sr.ReadString();
                        patients.sex = sr.ReadString();
                        patients.livingplace = sr.ReadString();
                        patients.age = sr.ReadInt32();
                        patients.diagnosis = sr.ReadString();
                        qpatients.Enqueue(patients);

                    }
                }
            }

            int chosen = -1, num;
            //пока не выйдем из проги
            while (chosen != 0)
            {
                chosen = Menu();
                switch (chosen)
                {
                    // прописываем логику по каждому пункту меню
                    case 1:
                        {
                            Console.Clear();
                            Patients p = new Patients();
                            Console.WriteLine("Введите фамилию пациента: ");
                            p.lastname = Console.ReadLine(); //
                            Console.WriteLine("Введите пол пациента (Мужчина/Женщина)");
                            p.sex = Console.ReadLine();//
                            while (true)
                            {
                                if (p.sex != "Мужчина" && p.sex != "Женщина")
                                {
                                    Console.WriteLine("Вы ввели не то!");
                                    Console.WriteLine("Введите пол пациента (Мужчина/Женщина)");
                                    p.sex = Console.ReadLine();

                                }

                                else break;
                            }
                            Console.WriteLine("Введите город проживания пациента:");
                            p.livingplace = Console.ReadLine();//
                            Console.WriteLine("Введите возраст пациента: ");
                            p.age = Convert.ToInt32(Console.ReadLine());//
                            while (true)
                            {
                                if (p.age <= 0)
                                {
                                    Console.WriteLine("Вы ввели некорректный возраст!");
                                    Console.WriteLine("Введите возраст пациента: ");
                                    p.age = Convert.ToInt32(Console.ReadLine());
                                }
                                else break;
                            }
                            Console.WriteLine("Введите диагноз пациента: ");
                            p.diagnosis = Console.ReadLine();
                            // добавляем элемент в очередь
                            qpatients.Enqueue(p);

                            break;
                        }

                    case 2:
                        {
                            Console.Clear();
                            int queueCount = qpatients.Count;
                            Console.WriteLine("ВВедите номер пациента, которого Вы хотите удалить из очереди: ");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            for (int i = 0; i < queueCount; i++)
                            {
                                // Текущий элемент из очереди постоянно удаляем и возвращаем обратно ТОЛЬКО те, которые не равны введенному номеру!
                                Patients currentFirstElement = qpatients.Dequeue(); 
                                if (i != choice - 1)
                                {
                                    qpatients.Enqueue(currentFirstElement);
                                }

                            }

                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            num = 0;
                            Console.WriteLine("Номер\tФамилия\tПол\tВозраст\tДиагноз\tГород проживания\t");
                            foreach (Patients c in qpatients)
                            {
                                Console.WriteLine($"{++num}\t{c.lastname}\t{c.sex}\t{c.age}\t{c.diagnosis}\t\t{c.livingplace}\t");
                            }
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            Console.WriteLine("Введите номер сотрудника, которого хотите изменить: ");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            int queueCount = qpatients.Count;

                            for (int i = 0; i < queueCount; i++)
                            {
                                
                                Patients currentFirst = qpatients.Dequeue();
                                if (i == choice - 1)
                                {
                                    
                                    Patients newVersionOfCurrnet = new Patients();
                                    Console.WriteLine(" Вы изменяете сотрудника под номером {0} ", choice);
                                    Console.WriteLine(" Выберите то, что вы хотите изменить:");
                                    bool quit = true;
                                    // флаги, для изменения информации в меню
                                    bool isdone = false;
                                    bool lastnameflag = false;
                                    bool diagflag = false;
                                    bool sexflag = false;
                                    bool livingplaceflag = false;
                                    bool ageflag = false;
                                    while (quit)
                                    {
                                        Console.Clear();
                                        if (lastnameflag)
                                            Console.WriteLine(" 1. Фамилия пациента [ИЗМЕНЕНО]");
                                        else
                                        Console.WriteLine(" 1. Фамилия пациента");
                                        if (diagflag)
                                            Console.WriteLine("2. Диагноз пациента [ИЗМЕНЕНО]");
                                        else
                                            Console.WriteLine(" 2. Диагноз пациента");
                                        if (sexflag)
                                            Console.WriteLine("3. Пол пациента [ИЗМЕНЕНО]");
                                        else
                                            Console.WriteLine(" 3. Пол пациента");
                                        if (livingplaceflag)
                                            Console.WriteLine(" 4. Город проживвания пациента [ИЗМЕНЕНО]");
                                        else
                                            Console.WriteLine(" 4. Город проживвания пациента");
                                        if (ageflag)
                                            Console.WriteLine(" 5. Возраст пациента [ИЗМЕНЕНО]");
                                        else
                                            Console.WriteLine(" 5. Возраст пациента");
                                        Console.WriteLine(" 6. Закончить изменения");
                                        int answer = Convert.ToInt32(Console.ReadLine());
                                        switch (answer)
                                        {
                                            case 1:
                                                {

                                                    Console.WriteLine(" Введите новую фамилию: ");
                                                    newVersionOfCurrnet.lastname = Console.ReadLine();
                                                    lastnameflag = true;
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    Console.WriteLine(" Введите новый диагноз ");
                                                    newVersionOfCurrnet.diagnosis = Console.ReadLine();
                                                    diagflag = true;
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    Console.WriteLine(" Введите новый пол");
                                                    newVersionOfCurrnet.sex = Console.ReadLine();
                                                    sexflag = true;
                                                    break;
                                                }
                                            case 4:
                                                {

                                                    Console.WriteLine(" Введите новый город проживания: ");
                                                    newVersionOfCurrnet.livingplace = Console.ReadLine();
                                                    livingplaceflag = true;
                                                    break;
                                                }
                                            case 5:
                                                {
                                                    Console.WriteLine(" Введите новый возраст ");
                                                    newVersionOfCurrnet.age = Convert.ToInt32(Console.ReadLine());
                                                    ageflag = true;
                                                    isdone = true;

                                                    break;
                                                }
                                            case 6:
                                                {
                                                    if (isdone)
                                                    {
                                                        Console.WriteLine(" Сотрудник успешно изменён!");
                                                        qpatients.Enqueue(newVersionOfCurrnet);
                                                        quit = false;
                                                    } else
                                                    {
                                                        Console.WriteLine(" Вы изменили не все поля! ");
                                                    }
                                                    break;
                                                }
                                        }
                                    }



                                }
                                else
                                {
                                    qpatients.Enqueue(currentFirst);
                                }
                            }

                            break;

                        }
                    case 5:
                        {
                            Console.Clear();
                            int howmany = 0;
                           
                            foreach (Patients c in qpatients)
                            {

                                
                                if (c.livingplace != "СПБ" && c.livingplace!="Санкт-Петербург")
                                {
                                    howmany++;
                                }

                            }
                            

                            Console.WriteLine("Количество иногородних пациентов, прибывших в клинику {0} ", howmany);
                            break;

                        }
                    case 6:
                        {
                            Console.Clear();

                            Console.WriteLine("Номер\tФамилия\tПол\tВозраст\tДиагноз\tГород проживания\t"); ;
                       
                            int number = 0;
                            foreach (Patients c in qpatients)
                            {
                                if (c.age >=60 && c.sex =="Женщина" || c.age >=65 && c.sex=="Мужчина")


                                {
                                    Console.WriteLine($"{++number}\t{c.lastname}\t{c.sex}\t{c.age}\t{c.diagnosis}\t\t{c.livingplace}\t");
                                }
                            }
                            break;
                        }
                    case 7:
                        {
                            Console.Clear();
                            int queueCount = qpatients.Count;
                            Console.WriteLine("Введите номер пациента, которого Вы хотите удалить из очереди с последующим уничтожением ");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            for (int i = 0; i < queueCount; i++)
                            {
                                // Текущий элемент из очереди постоянно удаляем и возвращаем обратно ТОЛЬКО те, которые не равны введенному номеру!
                                Patients currentFirstElement = new Patients();
                                        currentFirstElement =  qpatients.Dequeue();
                                if (i != choice - 1)
                                {
                                    qpatients.Enqueue(currentFirstElement);
                                }
                                if ( i == choice - 1)
                                {
                                    //уничтожаем текущий элемент
                                    currentFirstElement.Dispose();
                                    
                                    Console.WriteLine(" Пациент под номером {0} удалён из очереди и уничтожен!", choice);
                                }

                            }

                            break;
                        }
                    case 8:
                        {
                            Console.WriteLine(" Вызываем методы Dispose() для всех объектов очереди...");
                            Patients current = new Patients();
                            int icurrent = 0;
                            for (int i = 0; i < qpatients.Count; i++)
                            {
                                // получаем текущий элемент из очереди
                                current = qpatients.Dequeue();
                                // выполняем метод Dispose() у него
                                current.Dispose();
                                Console.WriteLine(" Вызыван метод у объекта под номером {0}...", ++icurrent);
                                // возвращем его обратно в очередь
                                qpatients.Enqueue(current);
                            }
                            if (qpatients.Count == icurrent )
                                Console.WriteLine("Методы Dispose() успешно вызваны у всех объектов очереди!");
                            break;
                        }
                    case 9:
                        {
                            Console.WriteLine(" Вызываем сборщик мусора...");
                            GC.Collect();
                            Console.WriteLine("Сборщик мусора успешно вызван!");
                            break;
                        }
                    case 10:
                        {
                            Console.WriteLine(" Выводим поколения у элементов очереди: ");
                            //создаем текущий элемент типа нашего класса, чтобы получать в него данные с верхнего элемента очереди
                            Patients current = new Patients();
                            int icurrent = 0;
                            for (int i=0; i<qpatients.Count; i++)
                            {
                                current = qpatients.Dequeue();
                                Console.WriteLine("Поколение элемента №{0} - {1}", ++icurrent, GC.GetGeneration(current));
                                qpatients.Enqueue(current);

                            }
                            Console.WriteLine("Вся память в программе: {0} ", GC.GetTotalMemory(false));
                            break;

                        }
                }
                // запись в бинарный файл ЧЕРЕЗ БИнариВрайтер, записываем в том порядке, в котором считывали
                //можно сделать через сериализацию
                using (BinaryWriter sr = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    foreach (Patients c in qpatients)
                    {
                        sr.Write(c.lastname);
                        sr.Write(c.sex);
                        sr.Write(c.livingplace);
                        sr.Write(c.age);
                        sr.Write(c.diagnosis);
                        
                    }
                }
                Console.ReadKey();
            }

        }
    }
}
