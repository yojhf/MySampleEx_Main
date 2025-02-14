using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 플레이어 공격 이펙트 플레이
    /// </summary>
    public class EllenStaffEffectSMB : StateMachineBehaviour
    {
        public int effectIndex;         //이펙트 인덱스 지정
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //지정 이펙트 애니메이션 플레이
            PlayerController playerController = animator.GetComponent<PlayerController>();
            playerController.meeleWeapon.effects[effectIndex].Activate();
        }
    }
}