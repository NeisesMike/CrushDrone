using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrushDrone
{
    public partial class Crush
    {
        public Color CrushYellow;
        public Color CrushBlack;
        public Color CrushNeutral = new Color(204f / 255f, 204f / 255f, 204f / 255f, 1f);
        public override void Awake()
        {
            base.Awake();
            Material[] CrushMaterials = transform.Find("Robot3/Drone.028").gameObject.GetComponent<MeshRenderer>().materials;
            CrushYellow = CrushMaterials.Where(x => x.name.Contains("black.001")).First().color;
            CrushBlack = CrushMaterials.Where(x => x.name.Contains("Material.007")).First().color;
        }
        private readonly List<string> CrushMainColor = new List<string>() { "black.001", "grey.001", "Material.002" };
        private readonly List<string> CrushStripeColor = new List<string>() { "black", "glass.001", "grey", "Material.003", "Material.006", "Material.007" };

        public override void SubConstructionComplete()
        {
            base.SubConstructionComplete();
            SetBaseColor(CrushYellow); // body
            SetStripeColor(CrushBlack); // accents
            SetInteriorColor(CrushNeutral); // arms
        }
        private void DoMaterial(MeshRenderer rend, List<string> matches, Color col)
        {
            var mats = rend.materials; // copy of the array, but references are what you will edit
            for (int i = 0; i < mats.Length; i++)
            {
                var n = mats[i].name.Replace(" (Instance)", "");
                if (matches.Contains(n))
                {
                    mats[i].color = col;
                }
            }
            rend.materials = mats; // important if you changed which materials go in which slots
        }
        private void DoMainMaterial(Color col)
        {
            var renderer = transform.Find("Robot3/Drone.028").GetComponent<MeshRenderer>();
            DoMaterial(renderer, CrushMainColor, col);
        }
        private void DoStripeMaterial(Color col)
        {
            var renderer = transform.Find("Robot3/Drone.028").GetComponent<MeshRenderer>();
            DoMaterial(renderer, CrushStripeColor, col);
        }
        protected override void PaintBaseColor(Vector3 hsb, Color color)
        {
            base.PaintBaseColor(hsb, color);
            DoMainMaterial(color);
        }
        protected override void PaintStripeColor(Vector3 hsb, Color color)
        {
            base.PaintStripeColor(hsb, color);
            DoStripeMaterial(color);
        }
        protected override void PaintInteriorColor(Vector3 hsb, Color color)
        {
            base.PaintInteriorColor(hsb, color);
            transform.Find("Robot3/body.Low").GetComponent<SkinnedMeshRenderer>().material.color = color;
            transform.Find("Robot3/body.Low.001").GetComponent<SkinnedMeshRenderer>().material.color = color;
        }
        protected override void PaintVehicleDefaultStyle()
        {
            base.PaintVehicleDefaultStyle();
            DoMainMaterial(CrushYellow);
            DoStripeMaterial(CrushBlack);
            transform.Find("Robot3/body.Low").GetComponent<SkinnedMeshRenderer>().material.color = CrushNeutral;
            transform.Find("Robot3/body.Low.001").GetComponent<SkinnedMeshRenderer>().material.color = CrushNeutral;
        }
    }
}
