using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Fighter fighter;
        Health health;
        //RaycastHit[] m_Results = new RaycastHit[1];

        void Start()
        {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }
        // bool InteractWithCombat()
        // {
        //     if(Physics.RaycastNonAlloc(GetMouseRay(), m_Results) > 0){
        //         for (int i = 0; i < m_Results.Length; i++)
        //         {
        //             CombatTarget target = m_Results[i].transform.gameObject.GetComponent<CombatTarget>();
        //             if(target == null) continue;

        //             if(Input.GetMouseButtonDown(0)){
        //                 fighter.Attack(target);
        //             } 
        //             return true;
        //         }
        //     } 
        //     return false;
        // }

        bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0)){
                    mover.StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            //Create raycast to locate hit point of the mouse
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
