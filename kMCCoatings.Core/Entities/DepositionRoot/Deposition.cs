using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.AtomRoot;

namespace kMCCoatings.Core.Entities.DepositionRoot
{
    public class Deposition
    {
        /// <summary>
        /// Концентрационный профиль поступающего потока, где номер массива - шаг итерации.
        /// Если шаг интеграции превышает размер массива, то поток считается циклическим.
        /// </summary>
        private readonly ElementConcentration[][] _concentrationFlow;

        /// <summary>
        /// Текущий шаг на участке концентрационного профиля.
        /// </summary>
        private int _step;

        /// <summary>
        /// Максимальный шаг концентрационного профиля, если <see cref="Deposition._step"/> превышает это значение, то
        /// оно обнуляется и считается начало нового цикла.
        /// </summary>
        private readonly int _maxStep;

        public Deposition(ElementConcentration[][] concentrationFlow, int step = -1)
        {
            _concentrationFlow = concentrationFlow;
            _step = step;
            _maxStep = concentrationFlow.GetLength(0) - 1;
        }

        /// <summary>
        /// Получить концентрационный профиль на следующем участке итерации
        /// </summary>
        public ElementConcentration[] MakeStep()
        {
            _step = _step == _maxStep ? 0 : _step + 1;
            return _concentrationFlow[_step];
        }
    }
}
