load barbie_girl;
load low_pass_coefficients; 
load differentiator_coefficients;

Fs=44100;
Ts = 1/Fs;

xL=filtfilt(bL,aL,x); % Application of the IIR filter

L = length(xL); 
W = 40;

% Calculating the energy of the signal

for k = 1:floor(L/W)
    xp = xL( (k-1)*W+1 : k*W );
    E(k) = sum(xp.^2);
end

Fs = 44100/W;
Ts = 1/Fs; 
Ef=filter(bD,1,E); % Application of the FIR filter

Edh=Ef;

% Rectifying the signal

for k=1:length(Edh)   
    if Edh(k)<=0
        Edh(k)=0;
    end     
end

expanded_t=t(1):(t(length(t))-t(1))/(length(Edh)-1):t(length(t)); % Arranging the time matrix for plotting

figure(1)
plot(expanded_t,Edh); % Plotting the rectified signal



AC=autocorr(Edh,1103,331,3); % Auto correlating the rectified signal
ACwoO=AC;
ACwoO(1)=0; % Neglecting the first value of the matrix, because it is the max value(1)

[value,index] = max(ACwoO); % Getting the maximum value's indice

bpm=round(60/((index-1)*Ts)); % Calculating the BPM

figure(2)
plot(AC); % Drawing of the auto correlation
xlim([331 length(AC)]); % Limiting the x-axis in the drawing

trainmat=zeros(size(Edh)); 

% Identifying the impulse train matrix
for i=1:length(Edh)
    if mod(i,index-1)==0
        trainmat(i)=1;
    end
end

CC=crosscorr(Edh,trainmat,1500); % Cros correlation of the rectified signal

figure(3)
crosscorr(Edh,trainmat,1500); % Drawing of the cross correlation
xlim([-1500 0]); % Limiting the x-axis in the drawing

max_val=304; % The first maximum is at -304 as can be seen from the cross correlation plot

time_start=max_val*Ts; % Calculating the starting time of the song 

fprintf('BPM = %d, Start = %0.2ds', bpm, time_start); % Printing the bpm and the starting time values
