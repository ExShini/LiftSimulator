public delegate void ButtonPush();

interface IButtonView
{
    event ButtonPush ButtonPushEvent;
}
