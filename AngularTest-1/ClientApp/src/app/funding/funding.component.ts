import { Component, OnInit } from '@angular/core';
import { FundingService } from '../services/funding.service';
import { FeatureFunding } from '../models/featurefunding';
import { FundingUser } from '../models/fundinguser';

@Component({
  selector: 'app-funding',
  templateUrl: './funding.component.html',
  styleUrls: ['./funding.component.css']
})
export class FundingComponent  {
  color = 'primary';
  mode = 'determinate';
  value=10;
  bufferValue = 45;
  step = 0;
  public featureFunding: FeatureFunding[] = new Array<FeatureFunding>();
  constructor(private fundingService: FundingService) {
    this.fundingService.getFeatureFundings().subscribe(result => {
      console.log('Result', result);
      this.featureFunding = result;
    });
  }
  submit(value: any) {
    var fundingUser = new FundingUser();
    console.log(value, 'dsadasdas');
    this.fundingService.addFunding(fundingUser).subscribe(result=>console.log(result));
  }
  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }
}
