using VzDev.ApiExtensions;
using VzDev.ObjectUtils;
using NaughtyAttributes;
using UnityEngine;

namespace VzDev.Advanced
{
    /// 把3D模型Pivot跟隨鼠標移動
    public class FollowMouse3D : MonoBehaviour
    {
        public int typeIndex = 0;
        [SerializeField] private LayerMask groundLayer;  // 指定地板

        public float fixedZ = 5f;  // 你的物件距離相機的 Z
        
        public Plane movePlane = new Plane(Vector3.up, Vector3.zero);
        
        void Update()
        {
            switch (typeIndex)
            {
                case 0:
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
                    {
                        transform.position = hit.point;
                    }
                    break;
                case 1:
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = fixedZ;

                    transform.position = Camera.main.ScreenToWorldPoint(mousePos);
                    break;
                case 2:
                    break;
            }   
        }
        
    }
}