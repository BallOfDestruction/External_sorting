# Внешняя сортировка

Реализация и тест внеших сортировок
Информацию можно найти по ссылке -> https://prog-cpp.ru/sort-merge/

## Информация для использования
Реализованные сортировки вы можете найти в проекте ExternalSort.<br>
Для использования сортировок вы должны имплементировать интерфейсы IExternalReader и IExternalWriter. <br>
Эти интерфейсы предоставляют доступ на запись и чтение вашего типа.<br>
Сортируемый тип вы должны создать в виде класса или структуры и обязательно имплементировать в нем функционал IComparable.<br>
Для примера реализован проект StudentData.<br><br>

## Алгоритмы

### Сортировка двухпутевым слиянием - TwoWayMerge

Исходная последовательность разбивается на две подпоследовательности.<br>
Эти две подпоследовательности объединяются в одну, содержащую упорядоченные пары.<br>
Полученная последовательность снова разбивается на две, и пары объединяются в упорядоченные четверки.<br>
Полученная последовательность снова разбивается на две и собирается в упорядоченные восьмерки.<br>
Данная операция повторяется до тех пор, пока полученная упорядоченная последовательность не будет иметь такой же размер, как у сортируемой.
