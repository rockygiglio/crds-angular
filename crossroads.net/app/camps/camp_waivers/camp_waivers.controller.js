export default class CampWaiversController {
  /* @ngInject */
  constructor() {
    this.signature = null;
    this.camper = {
      firstName: 'John',
      lastName: 'Doe'
    },
    this.waivers = [
      `
      <h4>MSM SUMMER CAMP LIABILITY RELEASE: RELEASE OF ALL CLAIMS</h4>
      <p>
      I, the undersigned parent or legal guardian for <strong>John Doe</strong>, do hereby release, forever discharge, and agree to indemnify and hold harmless
      Crossroads Church, its staff, board, volunteers and all representatives thereof from any liability, claims, or demands of any kind, including,
      but not limited to, personal injury, sickness, or death, as well as property damage and expenses of any nature whatsoever which may
      result from my child’s participation at <strong>HS Summer Camp at Woodland Lakes from June 13th to 17th</strong>.
      </p>
      `,
      `
      <h4>CAMP CHAUTAUQUA PARTICIPANT RELEASE</h4>
      <p>
      I, as the participant and/or the legal parent of a participant, do hereby indemnify and hold harmless Camp Chautauqua, and Buckeye Blitz
      Paintball, and their officers, directors, agents, employees, volunteers and representatives (the “Indemnified Parties”) from and against any
      and all liability, damages, actions, cause of action, claims, losses and/or expenses, including but not limited to attorney’s fees, court costs
      and expenses, arising in connections with or based on injury to or death of any persons or property, including the loss of use thereof,
      caused in whole or in part by any member of the Group or the Summer Camp Directorship, regardless of whether or not caused in whole
      or in part by the negligence of the indemnified parties, or any one or more of them. However, this indemnification shall not apply to willful
      misconduct committed by the Indemnified Parties. I understand that part of the camping experience involves activities and group living
      arrangements and interactions that may be new to myself or my child, and that they come with certain risks and uncertainties beyond
      what I and/or my child may be used to dealing with at home. I am aware of these risks, and I am assuming them (on behalf of my child). I
      realize that no environment is risk free, and so I acknowledge and/or have instructed my child on the importance of abiding by the camp’s
      rules, and I and my child both agree that I and he or she is familiar with these rules and will obey them.
      </p>
      `
    ]
  }

  getFullName() {
    return this.camper.firstName + ' ' + this.camper.lastName;
  }
}
