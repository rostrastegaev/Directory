import { NgModule } from '@angular/core';
import { AuthService } from './auth.service';
import { CommonModule } from '../commonModule/common.module';

@NgModule({
     imports: [CommonModule],
     providers: [AuthService],
     declarations: []
})
export class AuthModule {}