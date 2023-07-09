using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RGGD
{
    public sealed class PoolPoolCounters
    {
        private readonly HashSet<PictureBox> _pool = new HashSet<PictureBox>();
        private readonly Control.ControlCollection _controls;

        public PoolPoolCounters(Control.ControlCollection controls)
        {
            _controls = controls;
        }

        public PictureBox this[int index] => GetAtIndex(index);

        public bool Add(PictureBox poolCounter)
        {
            bool addResult = _pool.Add(poolCounter);

            if (addResult)
                _controls.Add(poolCounter);

            return addResult;
        }

        public void Remove(PictureBox poolCounter)
        {
            if (poolCounter == null)
                return;

            _pool.Remove(poolCounter);
            _controls.Remove(poolCounter);
        }

        public void Remove(int index) => Remove(GetAtIndex(index));

        private PictureBox GetAtIndex(int index)
        {
            try
            {
                return _pool.ElementAt(index);
            }
            catch
            {
                return null;
            }
        }
    }
}
