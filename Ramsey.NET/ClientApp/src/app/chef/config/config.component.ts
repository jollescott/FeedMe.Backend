import { Component, OnInit, ViewChild } from '@angular/core';
import { RamseyService } from '../../services/ramsey.service';
import { JsonEditorComponent, JsonEditorOptions } from 'ang-jsoneditor';

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styles: []
})
export class ConfigComponent implements OnInit {

  @ViewChild('synoEditor') synoEditor: JsonEditorComponent;
  @ViewChild('badEditor') badEditor: JsonEditorComponent;

  public editorOptions: JsonEditorOptions;

  constructor(private ramsey: RamseyService) {
    this.editorOptions = new JsonEditorOptions()
    this.editorOptions.modes = ['code', 'tree'];
    this.editorOptions.mode = 'code';
  }

  ngOnInit() {
    this.ramsey.getSyno().then(obj => {
      console.log(obj);
      this.synoEditor.set(obj);
    });

    this.ramsey.getBad().then(obj => {
      console.log(obj);
      this.badEditor.set(obj);
    });
  }

  async saveSyno() {
    await this.ramsey.saveSyno(this.synoEditor.get());
    alert("Update succeeded");
  }

  async saveBad() {
    await this.ramsey.saveBad(this.badEditor.get());
    alert("Update succeeded");
  }

  async reindex() {
    await this.ramsey.reindex();
    alert("Reindexation has been started");
  }

  async patch() {
    await this.ramsey.patch();
    alert("Database is being patched");
  }

}
