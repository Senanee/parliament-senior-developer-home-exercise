import { FormControl, ValidationErrors } from '@angular/forms';

export class MaxDateValidator {

   static GreterThanToday(control: FormControl): ValidationErrors | null {
        let today : Date = new Date();

       if (new Date(control.value) <= today)
           return { "GreterThanToday": true };

       return null;
   }
}