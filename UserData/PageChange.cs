using System.ComponentModel;


namespace UserData
{
    public class PageChange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static int countitems = 5; //количество объектов для отображения
        public int[] NPage { get; set; } = new int[countitems];//номер страницы, при нажатии на соответствующую кнопку    
        public string[] Visible { get; set; } = new string[countitems];//Массив свойст, отвечающий за видимость объекта Visible - видимый, Hidden - скрытый
        public string[] Bold { get; set; } = new string[countitems];//массив свойств, отвечающий за выделение текущей страницы
        int countpages;
        public int CountPages//количество страниц
        {
            get => countpages;
            set
            {
                countpages = value;
                for (int i = 1; i < countitems; i++)//делаем элементы управления невидимыми
                {
                    if (CountPages <= i) Visible[i] = "Hidden";//если страниц меньше, чем кнопок - скрываем лишние
                    else Visible[i] = "Visible";// а если их опять стало больше, то показываем назад
                }
            }
        }

        int countpage;//количество на странице
        public int CountPage  //количество на странице
        {
            get => countpage;
            set
            {
                countpage = value;
                if (Countlist % value == 0) CountPages = Countlist / value;//определение количества страниц
                else CountPages = 1 + Countlist / value;//если получается не целое еоличество страниц
            }
        }

        int countlist;
        public int Countlist //количество записей в листе
        {
            get => countlist;
            set
            {
                countlist = value;
                if (value % CountPage == 0) CountPages = value / CountPage;//определение количества страниц
                else CountPages = 1 + value / CountPage;
            }
        }
        int currentpage;//текущая страница
        public int CurrentPage
        {
            get => currentpage;
            set
            {
                currentpage = value;
                if (currentpage < 1) currentpage = 1;
                if (currentpage >= CountPages) currentpage = CountPages;
                //отрисовка                             
                for (int i = 0; i < countitems; i++)// рассматриваем три возможных случая
                {
                    if (currentpage < (1 + countitems / 2) || CountPages < countitems) NPage[i] = i + 1;//если страница в начале списка
                    else if (currentpage > CountPages - (countitems / 2 + 1)) NPage[i] = CountPages - (countitems - 1) + i;//если страница в конце списка
                    else NPage[i] = currentpage + i - (countitems / 2);//если страница в середине списка
                }
                for (int i = 0; i < countitems; i++)//выделяем активную страницу жирным
                {
                    if (NPage[i] == currentpage) Bold[i] = "ExtraBold";
                    else Bold[i] = "Regular";
                }
                //вызываем созбытие, связанное с изменением свойств, используемых в привязке на странице
                PropertyChanged(this, new PropertyChangedEventArgs("NPage"));
                PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
                PropertyChanged(this, new PropertyChangedEventArgs("Bold"));
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentPage"));
            }
        }
        public PageChange()
        {
            for (int i = 0; i < countitems; i++)
            {
                Visible[i] = "Visible";
                NPage[i] = i + 1;
                Bold[i] = "Regular";
            }
            currentpage = 1;
            countpage = 1;
            countlist = 1;
        }
    }
}
