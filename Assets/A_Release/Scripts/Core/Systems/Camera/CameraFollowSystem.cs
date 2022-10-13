using System;
using Leopotam.Ecs;
using MSFD;
using UnityEngine;
using UniRx;

namespace Commander.CameraSystem
{
    public class CameraFollowSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<CameraSettingData, TargetSourceData> _cameraFilter = null;
        public void Run()
        {
            foreach (var i in _cameraFilter)
            {
                ref CameraSettingData data = ref _cameraFilter.Get1(i);
                ref TargetSourceData targetSourceData = ref _cameraFilter.Get2(i);
                
                Vector3 avveragePosition = Vector3.zero;
                Vector3 desiredPosition = Vector3.zero;
                
                FindAveragePosition(data, targetSourceData.targets.ToArray(), out avveragePosition, out desiredPosition);
                Zoom(data, targetSourceData.targets.ToArray(), avveragePosition, desiredPosition, out desiredPosition);
                
                LimitPosition(data, desiredPosition, out desiredPosition);

                if (data.IsNeedToClampTargets)
                {
                    ClampTargetPos(data, targetSourceData.targets.ToArray());
                }
                
                Move(data, desiredPosition);
            }
        }
        private void Move(CameraSettingData data, Vector3 desiredPosition)
        {
            data.CameraRootTr.position = Vector3.SmoothDamp(data.CameraRootTr.position, desiredPosition, ref data.moveVelocity, data.DampTime);
        }
        private void Zoom(CameraSettingData data, Transform[] targets, Vector3 calculatedAvveragePos, Vector3 initialDesiredPosition, out Vector3 calculatedDesiredPosition)
        {
            float distanceFromCenter = 0;
            float length;

            foreach (Transform target in targets)
            {
                if (!target.gameObject.activeSelf)
                    continue;

                length = Vector3.Distance(target.position, calculatedAvveragePos);

                if (length >= distanceFromCenter)
                {
                    distanceFromCenter = length;
                }
            }
            
            float height = distanceFromCenter * data.HeightMultiplyer + data.ScreenEdgeBuffer;
            height = Mathf.Clamp(height, data.MinHeight, data.MaxHeight);
           
            calculatedDesiredPosition = new Vector3(initialDesiredPosition.x, height, initialDesiredPosition.z);
        }
        
        private void FindAveragePosition(CameraSettingData data, Transform[] targets, out Vector3 avveragePos, out Vector3 desired)
        {
            avveragePos = new Vector3();
            int numTargets = 0;

            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].gameObject.activeSelf)
                {
                    continue;
                }
                avveragePos += targets[i].position;
                numTargets++;
            }

            if (numTargets > 0)
            {
                avveragePos /= numTargets;
            }
            else
            {
                avveragePos = data.CameraRootTr.position;
            }
            
            avveragePos.y = 0;

            desired = avveragePos;
        }

        private void ClampTargetPos(CameraSettingData data, Transform[] targets)
        {
            foreach (Transform target in targets)
            {
                if (!target.gameObject.activeSelf)
                {
                    continue;
                }

                Vector3 vector00 = data.Camera.ScreenToWorldPoint(new Vector3(0, 0, data.CameraRootTr.position.y));
                Vector3 vector11 = data.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, data.CameraRootTr.position.y));
                
                Vector3 pos = target.position;

                pos.x = Mathf.Clamp(pos.x, vector00.x + data.ClampTargetBuffer, vector11.x - data.ClampTargetBuffer);
                pos.z = Mathf.Clamp(pos.z, vector00.z + data.ClampTargetBuffer, vector11.z - data.ClampTargetBuffer);

                target.position = pos;
            }
        }
        private void LimitPosition(CameraSettingData data, Vector3 desiredPos, out Vector3 calculatedDesired)
        {
            float yLength = data.CameraRootTr.position.y * Mathf.Tan(Mathf.Deg2Rad * data.Camera.fieldOfView / 2);
            float xLength = yLength * data.Camera.aspect;
            float xCoord = Mathf.Clamp(desiredPos.x, data.minXCoord + xLength, data.maxXCoord - xLength);
            float zCoord = Mathf.Clamp(desiredPos.z, data.minZCoord + yLength, data.maxZCoord - yLength);
            
            calculatedDesired = new Vector3(xCoord, desiredPos.y, zCoord);
        }
    }   
}
