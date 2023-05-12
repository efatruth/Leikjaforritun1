using UnityEngine;

// modified camera follow from unity sample assets
// breytt myndavél fylgja frá eignum einingasýnishorns

namespace UnitySampleAssetsModified
{

    public class Camera2DFollow : MonoBehaviour
    {

        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

		private Vector3 offsetY;
		private float offsetZ;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;

        // Use this for initialization
        // Notaðu þetta til að frumstilla
        private void Awake()
        {
            lastTargetPosition = target.position;

			// offset view height
            // offset view hæð
			offsetY = (transform.position - target.position);
			offsetY.x=0;
			offsetY.z=0;


            offsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }

        // Update is called once per frame
        // Uppfærsla er kölluð einu sinni í hvern ramma
        private void Update()
        {

            // only update lookahead pos if accelerating or changed direction
            // uppfærðu aðeins útlitsstöðu ef þú flýtir eða breytir um stefnu
            float xMoveDelta = (target.position - lastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                lookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

			Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward*offsetZ+offsetY;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

            transform.position = newPos;

            lastTargetPosition = target.position;
        }
    }
}