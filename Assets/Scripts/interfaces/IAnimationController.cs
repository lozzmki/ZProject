using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumAnimationCode
{
    ANIM_DEFAULT = 0,
    ANIM_MOVE,
    ANIM_RANGE_ATTACK,
    ANIM_MELEE_ATTACK,
    ANIM_DIE,
    ANIM_IDLE,
    ANIM_DODGE,

}
public interface IAnimationController
{
    //播放指定动画，bIfInterrupt指定是否打断当前播放中的动画。
    void playAnimation(enumAnimationCode nAnimation, bool bIfInterrupt = true);

    //停止播放动画，恢复到默认状态
    void stopAnimation();

    //设置动画播放速度
    void setTimeScale(float fScale = 1.0f);
}

