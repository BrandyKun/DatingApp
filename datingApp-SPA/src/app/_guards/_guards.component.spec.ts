/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { _guardsComponent } from './_guards.component';

describe('_guardsComponent', () => {
  let component: _guardsComponent;
  let fixture: ComponentFixture<_guardsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ _guardsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(_guardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
