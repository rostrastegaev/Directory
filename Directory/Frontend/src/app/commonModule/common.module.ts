import { NgModule } from '@angular/core';
import { HttpService } from './http.service';
import { StorageService } from './storage.service';
import { HttpModule } from '@angular/http';

@NgModule({
    imports: [HttpModule],
    providers: [HttpService, StorageService]
})
export class CommonModule {}