import { Component, OnInit } from '@angular/core';
import { RamseyService } from '../../services/ramsey.service';

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styles: []
})
export class ConfigComponent implements OnInit {

  synoJson: string;
  badJson: string;

  constructor(private ramsey: RamseyService) { }

  ngOnInit() {
  }

  saveSyno() {

  }

  saveBad() {

  }

  reindex() {
    console.log("index");
    this.ramsey.reindex();
  }

  patch() {

  }

}
