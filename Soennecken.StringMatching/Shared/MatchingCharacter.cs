using System;

namespace Soennecken.StringMatching.Shared
{
    public enum MatchingStatus
    {
        Outstanding,
        Matched,
        Mismatched
    }

    public class MatchingCharacter<T> : IEquatable<MatchingCharacter<T>>, IEquatable<T> where T : IEquatable<T>
    {
        public event Action OnChange;
        private readonly T _self;

        public MatchingStatus Status { get; private set; }

        public void Reset() => Status = MatchingStatus.Outstanding;

        public bool Equals(T other)
        {
            bool isEqual = _self.Equals(other);
            Update(isEqual);
            return isEqual;
        }

        public bool Equals(MatchingCharacter<T> other){
            var isEqual = this.Equals(other._self);
                other.Update(isEqual);
                return isEqual;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            MatchingCharacter<T> matchObj = obj as MatchingCharacter<T>;
            if (matchObj == null)
                return obj.Equals(this._self);
            else
                return this.Equals(matchObj);
        }

        public static bool operator ==(MatchingCharacter<T> a, MatchingCharacter<T> b)
        {
            if (((object)a) == null || ((object)b) == null)
                return Object.Equals(a, b);

            return a.Equals(b);
        }

        public static bool operator !=(MatchingCharacter<T> a, MatchingCharacter<T> b)
        {
            if (((object)a) == null || ((object)b) == null)
                return !Object.Equals(a, b);

            return !(a.Equals(b));
        }
        public override int GetHashCode()
        {
            return this._self.GetHashCode();
        }

        public override string ToString() => _self.ToString();
        public static MatchingCharacter<T>[] ToMatchingCharacter (T[] data)
        {
            var res = new MatchingCharacter<T>[data.Length];
            for (int i = 0; i < data.Length; i++)
                res[i] = new MatchingCharacter<T>(data[i]);
            return res;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        private void Update(bool isEqual)
        {
            if (isEqual)
                this.Status = MatchingStatus.Matched;
            else
                this.Status = MatchingStatus.Mismatched;

            NotifyStateChanged();
        }

        public MatchingCharacter(T self)
        {
            this._self = self;
        }
    }
}
