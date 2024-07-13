import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationErrorComponent } from './registration-error.component';

describe('RegistrationErrorComponent', () => {
  let component: RegistrationErrorComponent;
  let fixture: ComponentFixture<RegistrationErrorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrationErrorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrationErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
